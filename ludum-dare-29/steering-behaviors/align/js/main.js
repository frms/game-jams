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
	
	setSubmitEvent();
	
	camera = new Camera();
	
	gameObjects[0] = new Trapezoid();
	
	requestAnimationFrame(gameLoop);
});

function setSubmitEvent() {
	$('form').submit(function(e) {
		var orientation = $('#orientation').val();
		
		if(isNumber(orientation)) {
			gameObjects[1] = new Trapezoid([0, 0, -100], parseFloat(orientation) * (Math.PI/180));
		} else {
			gameObjects.splice(1, 1);
		}
		
		e.preventDefault();
	});
}


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
		var steering = align(gameObjects[0], gameObjects[1]);
		
		gameObjects[0].update(dt, steering);
		gameObjects[1].update(dt);
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

