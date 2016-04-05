timer.setTime(0);

/* Start network code */
var host = location.origin.replace(/^http/, 'ws');
var ws = new WebSocket(host);

var setUpData;

var worldState = {};
var battles = {};
var battlesSprites = {};

var gameOver = false;

//var networkStats = createStats(1, '100px', '0px');

ws.onmessage = function (event) {
	// networkStats.end();

	// networkStats.begin();

	var data = JSON.parse(event.data);

	// If we don't have a player then we haven't received our setupdata or created it yet
	if(!player) {
		// If this data has an id then it has our set up info, else we received a
		// world state broadcast before our set up info
		if(!setUpData && data.worldState) {
			setUpData = data;
		}

		return;
	}

	//console.log(event.data);

	if('remove' in data) {
		if(player.gameID === data.remove) {
			console.log('gameOver');
			var style = { font: "40px Arial", fill: "#ffffff", backgroundColor: '#000000', align: "center" };

			text = game.add.text(game.camera.position.x, game.camera.position.y, "Game Over\n Refresh browser to restart ", style);

			text.anchor.set(0.5);

			game.paused = true;

			gameOver = true;
			ws.close();
		}

		var entity = worldState[data.remove];
		if(entity) {
			entity.destroy();
		}
		delete worldState[data.remove];
	} 
	if('joinBattle' in data) {
		if(data.joinBattle !== 'no') {
			player.curBattle = data.joinBattle;

			handleBattle(data.battle);

			overworldGroup.visible = false;
			game.state.start('battle', false);
		}
	} 
	if('inBattle' in data) {
		if(data.players) {
			handleBattle(data);
		} else {
			delete battles[data.inBattle];

			if(battlesSprites[data.inBattle]) {
				battlesSprites[data.inBattle].destroy();
				delete battlesSprites[data.inBattle];
			}

			if(player.curBattle === data.inBattle) {
				leaveBattle();
			}
		}
	} 
	if('move' in data) {
		handleWorldMessage(data);
	}
	if('atk' in data) {
		if(player.curBattle === data.battle) {
			var e = battles[data.battle].enemies[data.target];

			if(e) {
				e.health -= data.dmg;
				
				if(e.health < 0) {
					battles[data.battle].enemies[data.target] = null;
				}

				battle.prototype.updateBattle();
			}
		}
	}
	if('eAtk' in data) {
		if(player.curBattle = data.eAtk) {
			var ph = battles[data.eAtk].playersHealth;

			for(var i = 0; i < data.atks.length; i++) {
				var a = data.atks[i];

				if(ph[a.target]) {
					ph[a.target] -= a.dmg;
					battle.prototype.updateBattle();
				}
			}
		}
	}
};

function handleBattle(data) {
	if(!battles[data.inBattle]) {
		var arr = data.inBattle.split(' ');
		battlesSprites[data.inBattle] = createBattleSprite(Number.parseInt(arr[0]), Number.parseInt(arr[1]));

		battles[data.inBattle] = data;
	}

	antonReplace(battles[data.inBattle], data);

	if(player.curBattle === data.inBattle && game.state.current === 'battle') {
		if(data.players.indexOf(player.gameID) === -1) {
			leaveBattle();
		} else {
			battle.prototype.updateBattle();
		}
	}
}

function leaveBattle() {
	player.curBattle = null;

	battleGroup.visible = false;
	game.state.start('overworld', false);
}

function antonReplace(oldObj, newObj) {
	for(var i in newObj) {
		oldObj[i] = newObj[i];
	}
}

function createBattleSprite(x, y) {
	var r = game.add.sprite((x+0.5)*map.tileWidth, (y+0.5)*map.tileHeight, 'battleIcon');
	r.anchor.set(0.5);
	overworldGroup.add(r);
	return r;
}

function handleWorldMessage(data) {
	var id = data.id;

	if(id !== player.gameID) {
		var entity = worldState[id];

		if(!entity) {
			entity = new OtherPlayer(id, data.move[0], data.move[1]);
			worldState[id] = entity;
		}

		entity.addTarget(new Phaser.Point(data.move[0], data.move[1]));
	}
}

ws.onclose = function() {
	if(!gameOver) {
		var style = { font: "40px Arial", fill: "#ffffff", backgroundColor: '#000000', align: "center" };

		text = game.add.text(game.camera.position.x, game.camera.position.y, "Connection Lost\n Refresh browser to restart ", style);

		text.anchor.set(0.5);

		game.paused = true;
	}
}


var now;
var lastTime;
var dt;

function updateWorldState() {
	now = timer.now();
	lastTime = lastTime || now;
	dt = (now - lastTime) / 1000.0;
	lastTime = now;

	// Update game
	for(var i in worldState) {
		worldState[i].move(now, dt);
	}
}

/* Stats code */
function createStats(mode, left, top) {
	var stats = new Stats();
	stats.setMode(mode);

	stats.domElement.style.position = 'absolute';
	stats.domElement.style.left = left;
	stats.domElement.style.top = top;

	document.body.appendChild( stats.domElement );

	return stats;
}

var fpsStats = createStats(0, '0px', '0px');

var game = new Phaser.Game(896, 504, Phaser.AUTO, '');
game.state.add("boot", boot);
game.state.add("overworld", overworld);
game.state.add("battle", battle);
game.state.start("boot");