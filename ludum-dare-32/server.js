var extend = require('extend');

performance = {
	now: require('performance-now')
};

var fs = require('fs');

eval(fs.readFileSync('./public/js/common.js', 'utf-8'));
timer.setTime(0);

eval(fs.readFileSync('./player.js', 'utf-8'));

var map = require('./loadMap.js');

var blockedLayer = map.getLayer('blockedLayer');
var objectsLayer = map.getLayer('objectsLayer');

// console.log(map.width, map.height, map.tileheight);
// console.log(blockedLayer.getTile(3,22));

var psData = objectsLayer.getObjects('playerStart')[0];
var playerStart = [psData.x + (map.tilewidth/2), psData.y + (map.tileheight/2)];
console.log(playerStart);


var WebSocketServer = require("ws").Server;
var http = require("http");
var express = require("express");
var app = express();
var port = process.env.PORT || 80;

app.use(express.static(__dirname + "/public"));

var server = http.createServer(app);
server.listen(port);

console.log("http server listening on %d", port);

var wss = new WebSocketServer({server: server});

wss.broadcast = function(data) {
	var str = JSON.stringify(data);

	for(var i in this.clients) {
		try {
			this.clients[i].send(str);
		} catch(e) {
			console.log(e);
		}
	}
};

function goodSend(data) {
	var str = JSON.stringify(data);

	try {
		this.send(str);
	} catch(e) {
		console.log(e);
	}
}

console.log("websocket server created");

var lastEntityID = -1;

var battles = {};
var rejectJoinBattleStr = {
	joinBattle: 'no'
};

wss.on("connection", function(ws) {
	console.log("websocket connection open");

	ws.goodSend = goodSend;

	var player = new Player();
	player.gameID = ++lastEntityID;
	player.position = [playerStart[0], playerStart[1]];
	player.curBattle = null;
	
	ws.player = player;

	var worldState = [];

	for(var i = 0; i < wss.clients.length; i++) {
		worldState[i] = wss.clients[i].player.getPlayerState();
	}

	var allState = player.getPlayerState();
	allState.worldState = worldState;

	var shortBattles = {};

	for(var i in battles) {
		shortBattles[i] = battles[i].getShortState();
	}

	allState.battles = shortBattles;

	ws.goodSend(allState);

	wss.broadcast(player.getPlayerState());

	ws.on('message', function(data, flags) {
		var msg = JSON.parse(data);

		if('move' in msg) {
			player.position = msg.move;
			wss.broadcast(player.getPlayerState());
		} else if('joinBattle' in msg) {
			handleJoinBattle(msg);
		} else if('atk' in msg) {
			handleAtk(msg);
		}

		console.log(data);
	});

	function handleJoinBattle(msg) {
		if(player.curBattle) {
			ws.goodSend(rejectJoinBattleStr);
			return;
		}

		var battleIndex = msg.joinBattle;

		var b = battles[battleIndex];

		if(!b) {
			b = new Battle(battleIndex);
			battles[battleIndex] = b;
		}

		if(b.players.length < MAX_IN_BATTLE) {
			b.addPlayer(player);

			ws.goodSend({
				joinBattle: battleIndex,
				battle: b //Anton
			});

			forPlayersInBattle(battleIndex, function(p, client) {
				client.goodSend(b);
			});

			wss.broadcast(b.getShortState());
		} else {
			ws.goodSend(rejectJoinBattleStr);
		}
	}

	function handleAtk(msg) {
		if(msg.atk === 'flee') {
			var b = player.removeFromBattle();

			if(b) {
				wss.broadcast(b.getShortState());
			}
		} else if(msg.atk === 'basic') {
			var b = battles[player.curBattle]

			if (b) {
				var e = b.enemies[msg.target];

				if(e) {
					e.health -= msg.dmg;

					if(e.health < 0) {
						b.enemies[msg.target] = null;

						if(!b.enemiesLeft()) {
							endBattle();
						}
					}

					forPlayersInBattle(player.curBattle, function(p, client) {
						client.goodSend(msg);
					});
				}
			}
		} else {
			endBattle();
		}
	}

	function endBattle() {
		var battleIndex = player.curBattle;

		forPlayersInBattle(battleIndex, function(p) {
			p.removeFromBattle();
		});

		wss.broadcast({
			inBattle: battleIndex,
			players: null
		});
	}

	ws.on("close", function() {
		console.log("websocket connection close");
		gameOverPlayer(player)
	});
});

function gameOverPlayer(player) {
	var msg = { remove: player.gameID };

	var b = player.removeFromBattle();

	if(b) {
		msg = extend(true, msg, b.getShortState());
	}

	wss.broadcast(msg);
}

function forPlayersInBattle(battleIndex, callback) {
	// Find all players in this battle
	for(var i = 0; i < wss.clients.length; i++) {
		var p = wss.clients[i].player;

		if(p.curBattle === battleIndex) {
			callback(p, wss.clients[i]);
		}
	}
}

// Main game loop
var lastTime = timer.now();
setInterval(function() {
	var now = timer.now();
	var dt = (now - lastTime) / 1000;
	lastTime = now;

	for(var i in battles) {
		var enemies = battles[i].enemies;

		var msg = {
			eAtk: i,
			atks: []
		};

		for(var j = 0; j < enemies.length; j++) {
			if(enemies[j]) {
				if(now - enemies[j].lastAtkTime > timeBetweenAtks) {
					var daAtk = {};
					daAtk.type='basic';
					daAtk.user = j;

					var randomPlayerIndex = Math.floor(Math.random() * battles[i].players.length);
					daAtk.target = battles[i].players[randomPlayerIndex];

					daAtk.dmg = 10 + Math.floor(Math.random() * 10);

					if(Math.random() < 0.2) {
						daAtk.dmg *= 2;
					}

					msg.atks.push(daAtk);

					battles[i].playersHealth[daAtk.target] -= daAtk.dmg;

					enemies[j].lastAtkTime = now;
				}
			}
		}

		if(msg.atks.length > 0) {
			forPlayersInBattle(i, function(p, client) {
				client.goodSend(msg);
			});
		}

		var ph = battles[i].playersHealth;

		for(var j in ph) {
			if(ph[j] < 0) {
				//Find and remove player from battle
				for(var k = 0; k < wss.clients.length; k++) {
					var p = wss.clients[k].player;

					if(Number.parseInt(p.gameID) === Number.parseInt(j)) {
						gameOverPlayer(p);
					}
				}
			}
		}
	}
}, 100);