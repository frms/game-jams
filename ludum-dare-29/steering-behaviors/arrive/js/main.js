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
	
	setKeyEvents();
	
	camera = new Camera();
	
	gameObjects[0] = new Trapezoid();
	
	requestAnimationFrame(gameLoop);
});

function setSubmitEvent() {
	$('form').submit(function(e) {
		var x = $('#x').val();
		var z = $('#z').val();
		
		if(isNumber(x) && isNumber(z)) {
			gameObjects[1] = new Circle([parseFloat(x), 0, parseFloat(z)]);
		} else {
			gameObjects.splice(1, 1);
		}
		
		e.preventDefault();
	});
}

function setKeyEvents() {
	$(document).keydown(function(e) {
		var target = gameObjects[1];
		
		if(target) {
			if(e.which == 37 && target.moveDirection[0] >= 0) {
				target.moveDirection[0] = -1;
			} else if(e.which == 39 && target.moveDirection[0] <= 0) {
				target.moveDirection[0] = 1;
			} else if(e.which == 38 && target.moveDirection[2] >= 0) {
				target.moveDirection[2] = -1;
			} else if(e.which == 40 && target.moveDirection[2] <= 0) {
				target.moveDirection[2] = 1;
			}
		}
	});
	
	$(document).keyup(function(e) {
		var target = gameObjects[1];
		
		if(target) {
			if(e.which == 37 && target.moveDirection[0] < 0) {
				target.moveDirection[0] = 0;
			} else if(e.which == 39 && target.moveDirection[0] > 0) {
				target.moveDirection[0] = 0;
			} else if(e.which == 38 && target.moveDirection[2] < 0) {
				target.moveDirection[2] = 0;
			} else if(e.which == 40 && target.moveDirection[2] > 0) {
				target.moveDirection[2] = 0;
			}
		}
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
		var steering = arrive(gameObjects[0], gameObjects[1]);
		
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

