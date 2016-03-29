"use strict";

/* Returns the steering for a character so it arrives at the target */
function arrive(character, target) {
	var steering = {};
	
	/* Get the right direction for the linear acceleration */
	var targetVelocity = vec3.sub([], target.position, character.position);
	
	/* Get the distance to the target */
	var dist = vec3.len(targetVelocity);
	
	/* If we are within the stopping radius then stop */
	if(dist < arrive.targetRadius) {
		return { linear: [0,0,0], angular:0, velocity: [0, 0, 0] };
	}
	
	/* Calculate the target speed, full speed at slowRadius distance and 0 speed at 0 distance */
	var targetSpeed;
	if(dist > arrive.slowRadius) {
		targetSpeed = character.maxSpeed;
	} else {
		targetSpeed = character.maxSpeed * (dist / arrive.slowRadius);
	}
	
	/* Give targetVelocity the correct speed */
	vec3.normalize(targetVelocity, targetVelocity);
	vec3.scale(targetVelocity, targetVelocity, targetSpeed);
	
	/* Calculate the linear acceleration we want */
	steering.linear = vec3.sub([], targetVelocity, character.velocity);
	/*
	 Rather than accelerate the character to the correct speed in 1 second, 
	 accelerate so we reach the desired speed in timeToTarget seconds 
	 (if we were to actually accelerate for the full timeToTarget seconds).
	*/
	vec3.scale(steering.linear, steering.linear, 1/arrive.timeToTarget);
	
	/* Make sure we are accelerating at max acceleration */
	if(vec3.len(steering.linear) > arrive.maxAcceleration) {
		vec3.normalize(steering.linear, steering.linear);
		vec3.scale(steering.linear, steering.linear, arrive.maxAcceleration);
	}
	
	steering.angular = 0;
	
	return steering;
}

arrive.maxAcceleration = 200;

/* The radius from the target that means we are close enough and have arrived */
arrive.targetRadius = 0.5;

/* The radius from the target where we start to slow down  */
arrive.slowRadius = 65;

/* The time in which we want to achieve the targetSpeed */
arrive.timeToTarget = 0.1;