"use strict";

/* The satisfaction radius to stop fleeing */
var radius = 300;

/* The time to target */
var timeToTarget = 0.25;

/* Returns the steering for a character so it seeks the target */
function seek(character, target) {
	var steering = {};
	
	/* Get the right direction for the velocity */
	steering.velocity = vec3.sub([], character.position, target.position);
	
	var dist = vec3.len(steering.velocity) ;
	
	/* If we are far enough from the target return no steering */
	if(dist > radius)  {
		return null;
	}
	
	/** 
	 * Calculate the speed. Either the max speed or the remaining 
	 * distance in timeToTarget seconds, which ever is less
	 */
	var speed = Math.min(character.maxSpeed, (radius - dist) / timeToTarget);
	
	/* Scale the velocity to the correct speed */
	vec3.normalize(steering.velocity, steering.velocity);
	vec3.scale(steering.velocity, steering.velocity, speed);
	
	/* Make the character face the direction we want him to face */
	if(vec3.squaredLength(steering.velocity)) {
		character.orientation = Math.atan2(steering.velocity[0], steering.velocity[2]);
	}
	
	steering.rotation = 0;
	
	return steering;
}