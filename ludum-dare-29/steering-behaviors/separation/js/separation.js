"use strict";

/* Returns the steering for a character so it seeks the target */
function separation() {
    var character = arguments[0];

	var steering = { linear: [0, 0, 0], angular: 0 };
	
    for(var i = 1; i < arguments.length; i++) {
        var separationTargets = arguments[i];
        
        for(var j = 0; j < separationTargets.length; j++) {
            if(separationTargets[j] == character) {
                continue;
            }
        
            /* Get the direction and distance from the target */
            var direction = vec3.sub([], character.position, separationTargets[j].position);
            var dist = vec3.len(direction);
            
            var threshold;
            
            if(separationTargets[j].size) {
                threshold = separationTargets[j].size;
            } else {
                threshold = separation.threshold;
            }
            
            if(dist < threshold) {
                /* Calculate the separation strength (can be changed to use inverse square law rather than linear) */
                var strength = separation.maxAcceleration * (separation.threshold - dist) / separation.threshold;
                
                /* Added separation acceleration to the existing steering */
                vec3.normalize(direction, direction);
                vec3.scale(direction, direction, strength);
                vec3.add(steering.linear, steering.linear, direction);
            }
        }
    }
	
	return steering;
}

/* Minimum distance at which separation happens */
separation.threshold = 50;

/* The maximum acceleration */
separation.maxAcceleration = 1000;