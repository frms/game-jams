"use strict";

/* Returns the steering for a character who is facing the target */
function face(character, target) {
	/* Calculate the direction to the target */
	var direction = vec3.sub([], target.position, character.position);
	
	/* Check for zero direction and return nothing */
	if(vec3.len(direction) == 0) {
		return { linear: [0,0,0], angular:0 };
	}
	
	/* Put the target together based with an orientation looking at the target */
	var explicitTarget = new GameObject();
	explicitTarget.orientation = Math.atan2(direction[0], direction[2]);
	
	return align(character, explicitTarget);
}