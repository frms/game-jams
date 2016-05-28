"use strict";

var host = location.origin.replace(/^http/, 'ws')
var ws = new WebSocket(host);

ws.onopen = function() {
	console.log('connected');
};

var count = 0;
var time = 0;

ws.onmessage = function (event) {
	var data = JSON.parse(event.data);
	if(typeof(data.removePlayer) != "undefined") {
		delete players[data.removePlayer];
		delete desiredPlayers[data.removePlayer];
		return;
	}
	
	if(data.map) {
		mapData = data;
		createMap();
		return;
	}
	
	you = data.you;
	
	desiredDt += data.dt;
	
	receivePlayers(data.players);
	receiveEnemies(data.enemies);
	
	var dt = Date.now() - parseInt(data.time);
	time += dt;
	count++;
	
	//console.log('Roundtrip time: ' + dt + 'ms');
	//console.log('Average time: ' + (time/count));
	//console.log('Delta Time between last message: ' + data.dt);
};

function receivePlayers(serverGameObjects) {
	receiveGameObjects(DuckMan, players, desiredPlayers, serverGameObjects);
}

function receiveEnemies(serverGameObjects) {
	receiveGameObjects(Enemy, enemies, desiredEnemies, serverGameObjects);
}

function receiveGameObjects(func, localGameObjects, desiredGameObjects, serverGameObjects) {
	for(var i in serverGameObjects) {
		var serverGameObject = serverGameObjects[i];
	
		if(typeof(localGameObjects[i]) == "undefined") {
			localGameObjects[i] = new func(serverGameObject.position);
		}
		desiredGameObjects[i] = serverGameObject.position;
		localGameObjects[i].lookDirection = serverGameObject.lookDirection;
		localGameObjects[i].moving = serverGameObject.moving;
		localGameObjects[i].attacking = serverGameObject.attacking;
		localGameObjects[i].health = serverGameObject.health;
		localGameObjects[i].death = serverGameObject.death;
	}
}

var tileSize = 64;
var sectionHeight = 9 * tileSize;
var sectionWidth = 16 * tileSize;

var sections;
var mapData;
var mapWidth;
var mapHeight;

function createMap() {
	if(!areTilesReady()) {
		setTimeout(createMap, 200);
		return;
	}

	mapHeight = mapData.map.length * sectionHeight;
	mapWidth = mapData.map[0].length * sectionWidth;
	
	sections = [];
	
	for(var i in mapData.sections) {
		var section = mapData.sections[i];
	
		/* Secondary canvas to create section images with */
		var m_canvas = document.createElement('canvas');
		m_canvas.width = sectionWidth;
		m_canvas.height = sectionHeight;
		var m_context = m_canvas.getContext('2d');
	
		for(var tileR in section) {
			for(var tileC in section[tileR]) {
				var x = tileC*tileSize;
				var y = tileR*tileSize;
				
				var img = tiles[section[tileR][tileC].tile];
				m_context.drawImage(img, x, y, img.width, img.height);
				
				if(section[tileR][tileC].wall == 2) {
					img = images['Rock'];
					m_context.drawImage(img, x, y, img.width, img.height);
				}
			}
		}
		
		sections.push(m_canvas);
	}
}

document.addEventListener("visibilitychange", function(event){
	if(document.hidden) {
		ws.send(JSON.stringify({
			time: null
		}));
	}
});

var screenWidth;
var screenHeight;

var context;
var fps;

var camera;

var players = {};
var desiredPlayers = {};
var enemies = {};
var desiredEnemies = {};

var desiredDt = 0;
var you = null;

$(document).ready(function() {
	$('#musicCheckbox').prop('checked', true);
	
	$('#musicCheckbox').change(function() {
		if(this.checked) {
			$('#music')[0].play();
		} else {
			$('#music')[0].pause();
		}
	});

	
	var canvas = $("#gameCanvas")[0];
	
	screenWidth = canvas.width;
	screenHeight = canvas.height;
	
	context = canvas.getContext("2d");
	context.font = "bold 14px sans-serif";
	
	fps = $("#fps")[0];
	
	setKeyEvents();
	
	camera = new Camera();
	
	requestAnimationFrame(gameLoop);
});

var moveDirection = [0, 0, 0];
var sendAttack = false;

var isKeyDown = {};

function setKeyEvents() {
	$(document).keydown(function(e) {
		if(e.which == 65 && moveDirection[0] >= 0) {
			moveDirection[0] = -1;
		} else if(e.which == 68 && moveDirection[0] <= 0) {
			moveDirection[0] = 1;
		} else if(e.which == 87 && moveDirection[2] >= 0) {
			moveDirection[2] = -1;
		} else if(e.which == 83 && moveDirection[2] <= 0) {
			moveDirection[2] = 1;
		} else if(e.which == 32 && !isKeyDown[32]) {
			isKeyDown[32] = true;
			sendAttack = true;
		}
	});
	
	$(document).keyup(function(e) {
		if(e.which == 65 && moveDirection[0] < 0) {
			moveDirection[0] = 0;
		} else if(e.which == 68 && moveDirection[0] > 0) {
			moveDirection[0] = 0;
		} else if(e.which == 87 && moveDirection[2] < 0) {
			moveDirection[2] = 0;
		} else if(e.which == 83 && moveDirection[2] > 0) {
			moveDirection[2] = 0;
		}
		
		isKeyDown[e.which] = false;
	});
}


var lastFrame = 0;

function gameLoop(timestamp) {
	updateFPS(timestamp);
	
	networkHeartbeat(timestamp);

    var dt = (timestamp - lastFrame) / 1000;

	lastFrame = timestamp;
    
    stepGame(dt, timestamp);
    drawGame();
	
	requestAnimationFrame(gameLoop);
};

/* Network Heartbeat */
var lastHeartbeatUpdate = 0;

function networkHeartbeat(timestamp) {
	if(ws.readyState == ws.OPEN) {
		var dt = (timestamp - lastHeartbeatUpdate) / 1000;
		
		if(dt > 0.05) {
			ws.send(JSON.stringify({
				time: Date.now(),
				moveDirection: moveDirection,
				attack: sendAttack
			}));
			
			if(sendAttack) {
				sendAttack = false;
			}
			
			lastHeartbeatUpdate = timestamp;
		}
	}
}


function stepGame(dt) {
	if(desiredDt > dt) {
		var t = dt / desiredDt;
		
		var sumDist = 0;
		
		for(var i in players) {
			vec3.lerp(players[i].position, players[i].position, desiredPlayers[i], t);
			sumDist += vec3.dist(players[i].position, desiredPlayers[i]);
		}
		for(var i in enemies) {
			vec3.lerp(enemies[i].position, enemies[i].position, desiredEnemies[i], t);
			sumDist += vec3.dist(enemies[i].position, desiredEnemies[i]);
		}
		
		if(sumDist < 1) {
			desiredDt = 0;
		} else {
			desiredDt -= dt;
		}
	} else {
		for(var i in players) {
			players[i].position = desiredPlayers[i];
		}
		for(var i in enemies) {
			enemies[i].position = desiredEnemies[i];
		}
		
		desiredDt = 0;
	}
}

function drawGame() {
	context.clearRect(0, 0, screenWidth, screenHeight);
	
	context.save();
	
	if(you != null) {
		camera.lookAt(players[you].position);
	}
	camera.applyTransforms();
	
	drawCoordinateGrid();
	
	drawMap();
	
	/*
	if(you != null) {
		context.rect(players[you].position[0],players[you].position[2]-110,185,155);
		context.stroke();
	}*/
	
	for(var i in enemies) {
		enemies[i].draw();
	}
	for(var i in players) {
		players[i].draw();
	}
	
	context.restore();
}

function drawMap() {
	if(mapData && sections) {
		for(var sectionR in mapData.map) {
			for(var sectionC in mapData.map[sectionR]) {
				var x = -1*(mapWidth/2) + sectionC*sectionWidth;
				var y = -1*(mapHeight/2) + sectionR*sectionHeight;
				
				var img = sections[mapData.map[sectionR][sectionC]];
				context.drawImage(img, x, y, img.width, img.height);
			}
		}
	}
}