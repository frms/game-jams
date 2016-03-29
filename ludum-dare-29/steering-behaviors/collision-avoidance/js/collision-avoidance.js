"use strict";

/**
 * Returns the steering for the character so he avoids all the targets.
 */
function collisionAvoidance(character, targets) {
	var steering = { linear: [0, 0, 0], angular: 0 };
	
	/* 1. Find the target that the character will collide with first */
	
	/* The first collision time */
	var shortestTime = Infinity;
	
	/* The first target that will collide and other data that
	we will need and can avoid recalculating */
	var firstTarget = null, firstMinSeparation, firstDistance, 
	firstDistance, firstRelativePos, firstRelativeVel;
	
	for(var i = 0; i < targets.length; i++) {
		
		/* Calculate the time to collision */
		var relativePos = vec3.sub([], character.position, targets[i].position);
		var relativeVel = vec3.sub([], character.velocity, targets[i].velocity);
		var distance = vec3.len(relativePos);
		var relativeSpeed = vec3.len(relativeVel);
		
		if(relativeSpeed == 0) {
			continue;
		}
		
		var timeToCollision = -1*vec3.dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);
		
		/* Check if they will collide at all */
		var separation = vec3.scale([], relativeVel, timeToCollision)
		vec3.add(separation, relativePos, separation);
		
		var minSeparation = vec3.len(separation);
		if(minSeparation > 2*collisionAvoidance.radius) {
			continue;
		}

		/* Check if its the shortest */
		if(timeToCollision > 0 && timeToCollision < shortestTime) {
			shortestTime = timeToCollision;
			firstTarget = targets[i];
			firstMinSeparation = minSeparation;
			firstDistance = distance;
			firstRelativePos = relativePos;
			firstRelativeVel = relativeVel;
		}
	}
	
	/* 2. Calculate the steering */
	
	/* If we have no target then exit */
	if(!firstTarget) {
		return steering;
	}
	
	/* If we are going to collide with no separation or if we are already colliding then 
	steer based on current position */
	var relativePos;
	if(firstMinSeparation <= 0 || firstDistance < 2*collisionAvoidance.radius) {
		vec3.sub(relativePos, character.position, firstTarget.position);
	}
	/* Else calculate the future relative position */
	else {
		vec3.scale(firstRelativeVel, firstRelativeVel, shortestTime);
		vec3.add(relativePos, firstRelativePos, firstRelativeVel);
	}
	
	/* Avoid the target */
	vec3.normalize(relativePos, relativePos);
	vec3.scale(steering.linear, relativePos, collisionAvoidance.maxAcceleration);
	
	return steering;
}

/* The maximum acceleration */
collisionAvoidance.maxAcceleration = 1000;

/* Holds the collision radius of a character (we assume all characters have the same radius) */
collisionAvoidance.radius = 25;