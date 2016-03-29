"use strict";

/* Returns the steering for a character who is pursuing a target */
function pursue(character, target) {
	/* Calculate the distance to the target */
	var displacement = vec3.sub([], target.position, character.position);
	var distance = vec3.len(displacement);
	
	/* Get the character's speed */
	var speed = vec3.len(character.velocity);
	
	/* Calculate the prediction time */
	var prediction;
	if(speed <= distance / pursue.maxPrediction) {
		prediction = pursue.maxPrediction;
	} else {
		prediction = distance / speed;
	}
	
	/* Put the target together based on where we think the target will be */
	var explicitTarget = new GameObject();
	vec3.scale(explicitTarget.position, target.velocity, prediction);
	vec3.add(explicitTarget.position, explicitTarget.position, target.position);
	
	return seek(character, explicitTarget);
}

/* Maximum prediction time the pursue will predict in the future */
pursue.maxPrediction = 1;