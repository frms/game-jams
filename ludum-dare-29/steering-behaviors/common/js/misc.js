"use strict";

/* FPS Counter Code */
var frames = 0;
var lastFpsUpdate = 0;

function updateFPS(timestamp) {
    frames++;
    var dtFPS = (timestamp - lastFpsUpdate) / 1000;
    if(dtFPS > 1.0) {
        fps.innerHTML = "FPS: " + Math.floor(frames/dtFPS);
        frames = 0;
        lastFpsUpdate = timestamp;
    }
}


/* Coordinate Grid Code */
var pixelsPerUnit = 10;

function drawCoordinateGrid() {
	/* Get the camera's top left corner position */
	var cameraTopLeft = [];
	cameraTopLeft[0] = -1*camera.translation[0];
	cameraTopLeft[1] = 0;
	cameraTopLeft[2] = -1*camera.translation[2];
	
	context.beginPath();
	
	/* Draw vertical lines */
	var x = cameraTopLeft[0] / pixelsPerUnit;
	x = Math.ceil(x)*pixelsPerUnit + 0.5;
	var maxX = x+screenWidth;
	
	for (; x < maxX; x += pixelsPerUnit) {
		context.moveTo(x, cameraTopLeft[2]);
		context.lineTo(x, cameraTopLeft[2] + screenHeight);
	}
	
	/* Draw horizontal lines */
	var z = cameraTopLeft[2] / pixelsPerUnit;
	z = Math.ceil(z)*pixelsPerUnit + 0.5;
	var maxZ = z+screenHeight;
	
	for (; z < maxZ; z += pixelsPerUnit) {
		context.moveTo(cameraTopLeft[0], z);
		context.lineTo(cameraTopLeft[0] + screenWidth, z);
	}
	
	context.strokeStyle = "#eee";
	context.stroke();
	
	drawAxes();
}

function drawAxes() {
	context.beginPath();
	
	context.moveTo(0, 0);
	context.lineTo(100, 0);
	context.moveTo(120, 0);
	context.lineTo(220, 0);
	context.moveTo(215, -5);
	context.lineTo(220, 0);
	context.lineTo(215, 5);
	
	context.moveTo(0, 0);
	context.lineTo(0, 100);
	context.moveTo(0, 120);
	context.lineTo(0, 220);
	context.moveTo(-5, 215);
	context.lineTo(0, 220);
	context.lineTo(5, 215);
	
	context.strokeStyle = "#000";
	context.stroke();
	
	context.fillText("x", 106, 4);
	context.fillText("z", -3, 114);
}


/* Tests a variable to see if its a number */
function isNumber(n) {
  return !isNaN(parseFloat(n)) && isFinite(n);
}


/* Helper method to help with javascript inheritance */
function inheritPrototype(subType, superType){
	/**
	 * Create a function with the same prototype as the superType,
	 * so TempFunc and superType are the same type.
	 */
	var TempFunc = function() {};
	TempFunc.prototype = superType.prototype;
	
	/**
	 * Use an object of type TempFunc for subType to inherit, because its the same 
	 * type as superType, but won't have any unused properties that a superType object might have 
	 */
	var subTypePrototype = new TempFunc();
	subTypePrototype.constructor = subType;
	subType.prototype = subTypePrototype;
}


/* Returns a random number between -1 and 1. Values around zero are more likely. */
function randomBinomial() {
	return Math.random() - Math.random();
}


/* Returns the orientation as a unit vector */
function orientationToVector(orientation) {
	return [Math.sin(orientation), 0, Math.cos(orientation)];
}


/* Given an angle in degrees this runs that angle in the -PI to PI range */
function mapToRange(angle) {
	/* Get the angle within the -2*PI and 2*PI range */
	angle = angle % (2*Math.PI);
	
	/* Get the angle within the -PI and PI range */
	if(angle > Math.PI) {
		angle -= 2*Math.PI;
	} else if(angle < -1*Math.PI) {
		angle += 2*Math.PI;
	}
	
	return angle;
}

/* Checks to see if the target is in front of the character */
function isInFront(character, target) {
    var facing = orientationToVector(character.orientation);
    
    var directionToTarget = vec3.sub([], target.position, character.position);
    vec3.normalize(directionToTarget, directionToTarget);
    
    return vec3.dot(facing, directionToTarget) >= 0;
    // return true;
}

/* Full screeen helper */
document.fullscreenEnabled = document.fullscreenEnabled || document.mozFullScreenEnabled || document.documentElement.webkitRequestFullScreen;

function requestFullscreen( element ) {
  if ( element.requestFullscreen ) {
    element.requestFullscreen();
  } else if ( element.mozRequestFullScreen ) {
    element.mozRequestFullScreen();
  } else if ( element.webkitRequestFullScreen ) {
    element.webkitRequestFullScreen( Element.ALLOW_KEYBOARD_INPUT );
  }
}

/* Blends together given steering and weights into a single steering */
function blendSteering() {
    var steering = { linear: [0, 0, 0], angular: 0 };
    
    var stopVelocity = false;
    
    for(var i = 0; i < arguments.length; i+=2) {
        vec3.scaleAndAdd(steering.linear, steering.linear, arguments[i].linear, arguments[i+1]);
        steering.angular +=  arguments[i].angular;
        
        if(arguments[i].velocity && arguments[i].velocity[0] == 0 && arguments[i].velocity[1] == 0 && arguments[i].velocity[2] == 0) {
            stopVelocity = true;
        }
    }
    
    if(vec3.len(steering.linear) <= 0.005) {
        steering.velocity = [0,0,0];
    }
    
    return steering;
}