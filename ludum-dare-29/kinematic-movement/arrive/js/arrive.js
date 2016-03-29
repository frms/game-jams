"use strict";

/* The satisfaction radius */
var radius = 3;

/* The time to target */
var timeToTarget = 0.25;

/* Returns the steering for a character so it arrives at the target */
function arrive(character, target) {
	var steering = {};
	
	/* Get the right direction for the velocity */
	steering.velocity = vec3.sub([], target.position, character.position);
	
	/* If we are close enough to the target return no steering */
	if(vec3.len(steering.velocity) < radius)  {
		return null;
	}
	
	/* We'd like for the character to reach the target in timeToTarget seconds */
	vec3.scale(steering.velocity, steering.velocity, 1/timeToTarget);
	
	/* If the velocity is going to fast then cap it at maxSpeed */
	if(vec3.len(steering.velocity) > character.maxSpeed)  {
		vec3.normalize(steering.velocity, steering.velocity);
		vec3.scale(steering.velocity, steering.velocity, character.maxSpeed);
	}
	
	/* Make the character face the direction we want him to face */
	if(vec3.squaredLength(steering.velocity)) {
		character.orientation = Math.atan2(steering.velocity[0], steering.velocity[2]);
	}
	
	steering.rotation = 0;
	
	return steering;
}