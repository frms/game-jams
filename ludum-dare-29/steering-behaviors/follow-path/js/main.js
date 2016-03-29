"use strict";

var screenWidth;
var screenHeight;

var context;
var fps;

var camera;
var gameObjects = [];

var path;

$(document).ready(function() {
	var canvas = $("#gameCanvas")[0];
	
	screenWidth = canvas.width;
	screenHeight = canvas.height;
	
	context = canvas.getContext("2d");
	context.font = "bold 14px sans-serif";
	
	fps = $("#fps")[0];
	
	camera = new Camera();
	
	gameObjects[0] = new Trapezoid();
	gameObjects[0].maxSpeed = 75;
	
	path = new Path([ 
		[150, 0, -100], 
		[250, 0, -100], 
		[350, 0, 0], 
		[400, 0, 0], 
		[465, 0, -150], 
		[500, 0, -230], 
		[200, 0, -270], 
		[-255, 0, -150], 
		[-400, 0, 200], 
		[-300, 0, 200], 
		[-250, 0, 270], 
		[-150, 0, 188], 
		[-50, 0, 270], 
		[50, 0, 188], 
		[150, 0, 270], 
		[250, 0, 188] 
	]);
	
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
	var steering = followPath(gameObjects[0], path);
	
	gameObjects[0].update(dt, steering);
}

function drawGame() {
	context.clearRect(0, 0, screenWidth, screenHeight);
	
	context.save();
	
	camera.applyTransforms();
	
	drawCoordinateGrid();
	
	path.draw();
	
	for(var i = 0; i < gameObjects.length; i++) {
		gameObjects[i].draw();
	}
	
	context.restore();
}

