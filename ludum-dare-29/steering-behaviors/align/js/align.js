"use strict";

/* Returns the steering for a character so it aligns with the target's orientation */
function align(character, target) {
	var steering = {};
	
	/* Get the naive orientation to the target */
	var rotation = target.orientation - character.orientation;
	
	/* Get rotation within the -pi and pi range */
	rotation = mapToRange(rotation);
	var rotationSize = Math.abs(rotation);
	
	/* If we are within the stopping radius then stop rotation */
	if(rotationSize <= align.targetRadius) {
		return { linear: [0,0,0], angular:0, rotation: 0 };
	}
	
	/* Calculate the target speed, full speed at slowRadius distance and 0 speed at 0 distance */
	var targetRotation;
	if(rotationSize > align.slowRadius) {
		targetRotation = character.maxRotation;
	} else {
		targetRotation = character.maxRotation * (rotationSize / align.slowRadius);
	}
	
	/* Give targetRotation the correct direction */
	targetRotation *= rotation / rotationSize;
	
	/* Calculate the linear acceleration we want */
	steering.angular = targetRotation - character.rotation;
	/*
	 Rather than accelerate the character to the correct speed in 1 second, 
	 accelerate so we reach the desired speed in timeToTarget seconds 
	 (if we were to actually accelerate for the full timeToTarget seconds).
	*/
	steering.angular /= align.timeToTarget;
	
	/* Make sure we are accelerating at max acceleration */
	var angularAcceleration = Math.abs(steering.angular);
	if(angularAcceleration > align.maxAngularAcceleration) {
		steering.angular /= angularAcceleration;
		steering.angular *= align.maxAngularAcceleration;
	}
	
	steering.linear = [0,0,0];
	
	return steering;
}

align.maxAngularAcceleration = 8*Math.PI;

/* The radius from the target that means we are close enough and are aligned */
align.targetRadius = Math.PI/500;

/* The radius from the target where we start to slow down  */
align.slowRadius = Math.PI/3;

/* The time in which we want to achieve the targetRotation */
align.timeToTarget = 0.1;