"use strict";

var screenWidth;
var screenHeight;

var context;
var fps;

var camera;

var character = new Trapezoid();
character.separationTargets = [ new Circle([100, 0, -30]), new Circle([250, 0, 30]), new Circle([350, 0, 150]) ];

var seekTarget;

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
	
	requestAnimationFrame(gameLoop);
});

function setSubmitEvent() {
	$('form').submit(function(e) {
		var x = $('#x').val();
		var z = $('#z').val();
		
		if(isNumber(x) && isNumber(z)) {
			seekTarget = new Circle([parseFloat(x), 0, parseFloat(z)]);
		} else {
			seekTarget = null
		}
		
		e.preventDefault();
	});
}

function setKeyEvents() {
	$(document).keydown(function(e) {
		var target = seekTarget;
		
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
		var target = seekTarget;
		
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
	if(seekTarget) {
		var steering = seek(character, seekTarget);
		
		var separationSteering = separation(character);
		
		if(vec3.len(separationSteering.linear) > 0) {
			steering = separationSteering;
		}
		
		character.update(dt, steering);
		seekTarget.update(dt);
	}
}

function drawGame() {
	context.clearRect(0, 0, screenWidth, screenHeight);
	
	context.save();
	
	camera.applyTransforms();
	
	drawCoordinateGrid();
	
	character.draw();
	
	for(var i = 0; i < character.separationTargets.length; i++) {
		character.separationTargets[i].draw();
	}
	
	if(seekTarget) {
		seekTarget.draw();
	}
	
	context.restore();
}

