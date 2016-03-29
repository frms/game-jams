"use strict";

/* Returns the steering for a character who is wandering */
function wander(character) {
	/* If the wander target does not exist then set it for the first time */
	if(!character.wanderTargetPosition) {
		character.wanderTargetPosition = vec3.scale([], orientationToVector(character.orientation), wander.wanderOffset);
		vec3.add(character.wanderTargetPosition, character.wanderTargetPosition, character.position);
	}
	
	/* Get the direction to the last location of the wander target */
	var direction = vec3.sub([], character.wanderTargetPosition, character.position);
	vec3.normalize(direction, direction);
	
	/* Set the center of the new wander square in the same direction as the
	last wander location at an offset distance away from the character */
	character.wanderTargetPosition = vec3.scale([], direction, wander.wanderOffset);
	vec3.add(character.wanderTargetPosition, character.position, character.wanderTargetPosition);
	
	/* Randomly move the wander target somewhere within the wander square */
	character.wanderTargetPosition[0] += randomBinomial() * wander.wanderHalfLength;
	character.wanderTargetPosition[2] += randomBinomial() * wander.wanderHalfLength;
	
	/* Seek the new wander target location */
	var steering = seek(character, new GameObject(character.wanderTargetPosition));
	
	/* Make sure we are looking what ever direction we are moving */
	var lookSteering = lookWhereYoureGoing(character);
	steering.angular = lookSteering.angular;
	steering.rotation = lookSteering.rotation;
	
	return steering;
}

/* Use numbers that are a good bit larger than the character's speed for the 
offset and half length, because if they are too small the character will move 
so quickly that the target could end up behind the character and the algorithm 
always assumes the target is ahead of the character (and bases the new target
 location on the last target location). If they are too small you will see a lot
 of turning.

The ratio of offset to half length determines how much the character will 
turn in its wandering. The larger the half length compared to the offset 
the more turny the character will be.
 */

/* The forward offset of the wander square */
wander.wanderOffset = 1000;

/* The 1/2 length of the wander square */
wander.wanderHalfLength = 300;