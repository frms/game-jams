"use strict";

/**
 * Creates a camera object.
 * In 2D the translation property of the camera object is the inverse of the position 
 * of the camera frame's top left corner.
 */ 
function Camera() {
	this.translation = [];
	this.lookAt([0,0,0]);
}

/* Makes the camera look at the given point. In the 2D case it makes the camera centered at the given point */
Camera.prototype.lookAt = function(point) {
	this.translation[0] = Math.floor(screenWidth/2 - point[0]);
	this.translation[1] = 0;
	this.translation[2] = Math.floor(screenHeight/2 - point[2]);
};

/* Applies the camera transforms to the canvas */
Camera.prototype.applyTransforms = function() {
	context.translate(this.translation[0], this.translation[2]);
};