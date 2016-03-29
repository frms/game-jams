"use strict";

var screenWidth;
var screenHeight;

var context;
var fps;

var camera;

var character;

/* Targets to avoid */
var targets = [];

var arriveTarget;

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
	
	character = new Trapezoid([0,0,250]);
	createTargets();
	
	requestAnimationFrame(gameLoop);
});

function setSubmitEvent() {
	$('form').submit(function(e) {
		var x = $('#x').val();
		var z = $('#z').val();
		
		if(isNumber(x) && isNumber(z)) {
			arriveTarget = new Circle([parseFloat(x), 0, parseFloat(z)]);
		} else {
			arriveTarget = null;
		}
		
		e.preventDefault();
	});
}

function setKeyEvents() {
	$(document).keydown(function(e) {
		var target = arriveTarget;
		
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
		var target = arriveTarget;
		
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

function createTargets() {
	for(var i = 0; i < 4; i++) {
		var z = -200 + 100*i;
	
		targets[i] = new Trapezoid([-150,0,z]);
		targets[i].path = new Path([ 
			[-150, 0, z], 
			[150, 0, z]
		]);
		targets[i].pathLoop = true;
	}
	
	targets[0].maxSpeed = 75;
	targets[1].maxSpeed = 95;
	targets[2].maxSpeed = 150;
	targets[3].maxSpeed = 120;
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
	for(var i = 0; i < targets.length; i++) {
		var steering = followPath(targets[i], targets[i].path);
		targets[i].update(dt, steering);
	}
	
	/* Steer the character and arrive target */
	var steering1 = collisionAvoidance(character, targets);
	if(arriveTarget) {
		if(vec3.len(steering1.linear) == 0) {
			steering1 = arrive(character, arriveTarget);
		}
		
		var steering2 = lookWhereYoureGoing(character);
		steering1.angular = steering2.angular;
		steering1.rotation = steering2.rotation;
		
		arriveTarget.update(dt);
	}
	character.update(dt, steering1);
}

function drawGame() {
	context.clearRect(0, 0, screenWidth, screenHeight);
	
	context.save();
	
	camera.applyTransforms();
	
	drawCoordinateGrid();
	
	for(var i = 0; i < targets.length; i++) {
		targets[i].draw();
	}
	
	character.draw();
	
	if(arriveTarget) {
		arriveTarget.draw();
	}
	
	context.restore();
}

