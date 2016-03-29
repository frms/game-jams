"use strict";

var screenWidth;
var screenHeight;

var context;
var fps;

var camera;
var gameObjects = [];

$(document).ready(function() {
	var canvas = $("#gameCanvas")[0];
	
	screenWidth = canvas.width;
	screenHeight = canvas.height;
	
	context = canvas.getContext("2d");
	context.font = "bold 14px sans-serif";
	
	fps = $("#fps")[0];
	
	$('#flee').submit(function(e) {
		var x = $('#x').val();
		var z = $('#z').val();
		
		if(isNumber(x) && isNumber(z)) {
			gameObjects[1] = new Circle([x, 0, z]);
		} else {
			gameObjects.splice(1, 1);
		}
		
		e.preventDefault();
	});
	
	camera = new Camera();
	
	gameObjects[0] = new Trapezoid();
	
	requestAnimationFrame(gameLoop);
});


var lastFrame = 0;

function gameLoop(timestamp) {
	updateFPS(timestamp);

    var dt = (timestamp - lastFrame) / 1000;
    dt = (dt > 1/15) ? 1/15 : dt; 
	lastFrame = timestamp;
    
    stepGame(dt, timestamp);
    drawGame();
	
	requestAnimationFrame(gameLoop);
};

function stepGame(dt) {
	if(gameObjects[1]) {
		var steering = seek(gameObjects[0], gameObjects[1]);
		
		if(steering) {
			vec3.scale(steering.velocity, steering.velocity, dt);
			vec3.add(gameObjects[0].position, gameObjects[0].position, steering.velocity);
		}
	}
}

function drawGame() {
	context.clearRect(0, 0, screenWidth, screenHeight);
	
	context.save();
	
	camera.applyTransforms();
	
	drawCoordinateGrid();
	
	for(var i = 0; i < gameObjects.length; i++) {
		gameObjects[i].draw();
	}
	
	context.restore();
}

