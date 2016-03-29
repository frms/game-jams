"use strict";

/* The maximum rotation the character can wander */
var maxRotation = Math.PI * 6;

/* Returns the steering for character to randomly wander */
function wander(character) {
	var steering = {};
	
	/* Have the character go full speed in the direction he is facing */
	steering.velocity = orientationToVector(character.orientation);
	vec3.scale(steering.velocity, steering.velocity, character.maxSpeed);
	
	/* Randomly decide the wander rotation */
	steering.rotation =  randomBinomial() * maxRotation;
	
	return steering;
}