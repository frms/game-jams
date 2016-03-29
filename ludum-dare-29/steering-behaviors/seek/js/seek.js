"use strict";

/* Returns the steering for a character so it seeks the target. The
 maxAcceleration parameter is optional */
function seek(character, target, maxAcceleration) {
	/* If maxAcceleration is not given use the default value */
	if(typeof maxAcceleration == 'undefined') {
		maxAcceleration = seek.maxAcceleration
	}

	var steering = {};
	
	/* Get the right direction for the linear acceleration */
	steering.linear = vec3.sub([], target.position, character.position);
	
	vec3.normalize(steering.linear, steering.linear);
	vec3.scale(steering.linear, steering.linear, maxAcceleration);
	
	
	steering.angular = 0;
	
	return steering;
}

/* The maximum acceleration */
seek.maxAcceleration = 200;