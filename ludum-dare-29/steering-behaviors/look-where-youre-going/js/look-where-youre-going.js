"use strict";

/* Returns the steering for a character who is facing the same direction as its velocity */
function lookWhereYoureGoing(character) {	
	/* Check for zero direction and return nothing and stop rotation */
	if(vec3.len(character.velocity) == 0) {
		return { linear: [0,0,0], angular:0, rotation: 0 };
	}
	
	/* Put the target together based with an orientation looking at where the character is going */
	var explicitTarget = {};
	explicitTarget.orientation = Math.atan2(character.velocity[0], character.velocity[2]);
	
	return align(character, explicitTarget);
}

/* Returns the steering for a character who is facing the same direction as its velocity */
function lookWhereYoureGoing2(character, dt) {	
	/* Check for zero direction and return nothing and stop rotation */
	if(vec3.len(character.velocity) == 0) {
		return { linear: [0,0,0], angular:0, rotation: 0 };
	}
	
	/* Put the target together based with an orientation looking at where the character is going */

	var orientation = Math.atan2(character.velocity[0], character.velocity[2]);
    
    	/* Get the naive orientation to the target */
	var rotation = orientation - character.orientation;
	
	/* Get rotation within the -pi and pi range */
	rotation = mapToRange(rotation);
	
    character.orientation += rotation*5*dt;
    
    return { linear: [0,0,0], angular:0 };
}