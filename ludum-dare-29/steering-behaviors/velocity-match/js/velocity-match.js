"use strict";

/* Returns the steering for a character so it matches the velocity of the target */
function velocityMatch(character, target) {
	var steering = { linear: [0, 0, 0], angular: 0 };
	
	/* Calculate the linear acceleration we want */
	steering.linear = vec3.sub([], target.velocity, character.velocity);
	/*
	 Rather than accelerate the character to the correct speed in 1 second, 
	 accelerate so we reach the desired speed in timeToTarget seconds 
	 (if we were to actually accelerate for the full timeToTarget seconds).
	*/
	vec3.scale(steering.linear, steering.linear, 1/velocityMatch.timeToTarget);
	
	/* Make sure we are accelerating at max acceleration */
	if(vec3.len(steering.linear) > velocityMatch.maxAcceleration) {
		vec3.normalize(steering.linear, steering.linear);
		vec3.scale(steering.linear, steering.linear, velocityMatch.maxAcceleration);
	}
	
	return steering;
}

velocityMatch.maxAcceleration = 200;

/* The time in which we want to achieve the targetSpeed */
velocityMatch.timeToTarget = 0.1;

/* Returns the steering for a character so it matches the average velocity of a group of targets */
function velocityMatchTargets(character, targets, ignorePlayer) {
    var steering = { linear: [0, 0, 0], angular: 0 };
    var count = 0;

    for(var i in targets) {
        if(ignorePlayer && targets[i] instanceof Player) {
            continue;
        }
    
        if(character != targets[i] && (vec3.dist(character.position, targets[i].position) < velocityMatchTargets.threshold) && isInFront(character, targets[i])) {
            /* Calculate the linear acceleration we want */
            var linear = vec3.sub([], targets[i].velocity, character.velocity);
            /*
             Rather than accelerate the character to the correct speed in 1 second, 
             accelerate so we reach the desired speed in timeToTarget seconds 
             (if we were to actually accelerate for the full timeToTarget seconds).
            */
            vec3.scale(linear, linear, 1/velocityMatch.timeToTarget);
            
            vec3.add(steering.linear, steering.linear, linear);
            
            count++;
        }
    }
    
    if(count > 0) {
        vec3.scale(steering.linear, steering.linear, 1/count);
        
        /* Make sure we are accelerating at max acceleration */
        if(vec3.len(steering.linear) > velocityMatch.maxAcceleration) {
            vec3.normalize(steering.linear, steering.linear);
            vec3.scale(steering.linear, steering.linear, velocityMatch.maxAcceleration);
        }
    }
    
    return steering;
}

/* Minimum distance at which velocityMatchTargets happens */
velocityMatchTargets.threshold = 200;