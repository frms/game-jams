"use strict";

/* Calculates the center of mass of the given targets and returns the arrive steering */
function cohesion(character, targets, ignorePlayer) {
    var centerOfMass = [0,0,0];
    var count = 0;
    
    /* Sums up everyone's position who is close enough and in front of the character */
    for(var i in targets) {
        if(ignorePlayer && targets[i] instanceof Player) {
            continue;
        }
    
        if( (vec3.dist(character.position, targets[i].position) < cohesion.threshold) && isInFront(character, targets[i]) ){
            vec3.add(centerOfMass, centerOfMass, targets[i].position);
            count++;
        }
    }
    
    /* If we find targets who are close and in front then return the center of mass */
    if(count > 0) {
        vec3.scale(centerOfMass, centerOfMass, 1/count);
        return arrive(character, {position: centerOfMass});
    } else {
        return { linear: [0, 0, 0], angular: 0 };
    }
}

/* Minimum distance at which cohesion happens */
cohesion.threshold = 200;

