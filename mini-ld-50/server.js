var fs = require('fs');
var vm = require('vm');

function include(path) {
    var code = fs.readFileSync(path, 'utf-8');
    vm.runInThisContext(code, path);
}

include('./js/helper/gl-matrix.js');
include('./js/helper/misc.js');
eval(fs.readFileSync('./game-update.js')+'');
include('./mapSections.js');

var WebSocketServer = require('ws').Server
  , http = require('http')
  , express = require('express')
  , app = express()
  , port = process.env.PORT || 5000;

app.use(express.static(__dirname + '/'));

var server = http.createServer(app);
server.listen(port);

console.log('http server listening on %d', port);

var wss = new WebSocketServer({server: server});

wss.broadcast = function(data) {
    for(var i in this.clients)
        this.clients[i].send(data);
};

console.log('websocket server created');

var map = [
	[ 0, 1, 2, 0, 1, 2 ],
	[ 0, 1, 2, 0, 1, 2 ],
	[ 0, 1, 2, 0, 1, 2 ],
	[ 0, 1, 2, 0, 1, 2 ],
	[ 2, 3, 0, 0, 1, 2 ],
	[ 1, 0, 2, 0, 1, 2 ]
];

var tileSize = 64;
var sectionHeight = 9 * tileSize;
var sectionWidth = 16 * tileSize;

var mapWidth;
var mapHeight;

mapHeight = map.length * sectionHeight;
mapWidth = map[0].length * sectionWidth;

var players = {};
var nextPlayerIndex = 0;

function pushPlayer(player) {
	players[nextPlayerIndex] = player;
	nextPlayerIndex++;
	
	/* Return player's position */
	return nextPlayerIndex-1;
}

var enemies = {};
var nextEnemyIndex = 0;

function pushEnemy(enemy) {
	enemies[nextEnemyIndex] = enemy;
	nextEnemyIndex++;
	
	/* Return enemy's position */
	return nextEnemyIndex-1;
}

createEnemies();

function createEnemies() {
	pushEnemy(new Enemy([-300, 0, 0]));
	pushEnemy(new Enemy([600, 0, 0]));
}

setInterval(stepEnemies, 30);

function getClientData(gameObjects, userTime) {
	var hashMap = {}
	for(var i in gameObjects) {
		var death = 0;
		
		if(gameObjects[i].deathTime >= 0) {
			death = 1;
			if((Date.now() - gameObjects[i].deathTime) > 300) {
				death = 2;
			}
		}
		
		hashMap[i] = {
			position: gameObjects[i].position,
			lookDirection: gameObjects[i].lookDirection,
			moving: gameObjects[i].moving,
			attacking: (gameObjects[i].atkTime > 0),
			health: gameObjects[i].health,
			death: death
		};
	}
	return hashMap;
}


wss.on('connection', function(ws) {
    console.log('websocket connection open');
	
	ws.send(JSON.stringify({
		map: map,
		sections: sections
	}), function() {  });
	
	var lastTime = null;
	
	var playerIndex = pushPlayer(new Player());
	var player = players[playerIndex];
	
	ws.on('message', function(data) {
        //console.log('received: %s', data);
		data = JSON.parse(data);
		
		if(data.time == null) {
			//console.log("Got null time going to reset lastTime");
			lastTime = null;
			return;
		}
		
		var dt = null;
		
		if(lastTime != null) {
			dt = (data.time - lastTime) / 1000;
			
			playerUpdate(player, data.moveDirection, data.attack, dt);
			//console.log(player);
		}
		
		lastTime = data.time;
		
		//console.log(lastTime);
		
		ws.send(JSON.stringify({
			time: data.time,
			dt: dt,
			you: playerIndex,
			players: getClientData(players, data.time),
			enemies: getClientData(enemies, data.time)
		}), function() {  });
    });

    ws.on('close', function() {
        console.log('websocket connection close');
		
		delete players[playerIndex];
		
		wss.broadcast(JSON.stringify({removePlayer:playerIndex}));
    });
});
