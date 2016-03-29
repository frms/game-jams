"use strict";

/* Combines cohesion, separation, and velocity match to make the character flock towards other targets */
function flocking(character, targets, obstacles) {
    var ignorePlayer = (character instanceof Enemy);

    var cohesionSteering = cohesion(character, targets, ignorePlayer);
    
    var separationSteering = separation(character, targets, obstacles);
    
    var velMatchSteering = velocityMatchTargets(character, targets, ignorePlayer);
    
    var steering = blendSteering(cohesionSteering, 1.5, separationSteering, 2, velMatchSteering, 1);
    
    var lookWhereGoingSteering = lookWhereYoureGoing(character);
    
    steering.angular = lookWhereGoingSteering.angular;
    steering.rotation = lookWhereGoingSteering.rotation;
    
    return steering;
}