"use strict";

/* Returns the steering for a character so it flees the target */
function flee(character, target) {
	var steering = {};
	
	/* Get the right direction for the linear acceleration */
	steering.linear = vec3.sub([], character.position, target.position);
	
	vec3.normalize(steering.linear, steering.linear);
	vec3.scale(steering.linear, steering.linear, flee.maxAcceleration);
	
	
	steering.angular = 0;
	
	return steering;
}

/* The maximum acceleration */
flee.maxAcceleration = 200;