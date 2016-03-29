"use strict";

function followPath(character, path) {	
	/* Set the path direction if it is not already set */
	if(!character.pathDirection) {
		character.pathDirection = 1;
	}
	
	/* Find the final destination of the character on this path */
	var finalDestination = (character.pathDirection > 0) ? path.nodes[path.nodes.length-1] : path.nodes[0];
	
	/* If we are close enough to the final destination then either stop moving or reverse if 
	the character is set to loop on paths */
	if( vec3.dist(character.position, finalDestination) < followPath.stopRadius ) {
		if(character.pathLoop) {
			character.pathDirection *= -1;
		} else {
			return { linear: [0,0,0], angular:0, velocity: [0, 0, 0], rotation: 0 };
		}
	}
	
	/* Get the param for the closest position point on the path given the character's position */
	var param = path.getParam(character.position);
	
	/* Move down the path */
	param += character.pathDirection * followPath.pathOffset;
	
	/* Make sure we don't move past the beginning or end of the path */
	if (param < 0) {
		param = 0;
	} else if (param > path.maxDist) {
		param = path.maxDist;
	}
	
	/* Create the target */
	var explicitTarget = new GameObject(path.getPosition(param));
	
	/* Seek and Look Where You're Going */
	var steering1 = seek(character, explicitTarget);		
	var steering2 = lookWhereYoureGoing(character);
		
	steering1.angular = steering2.angular;
	steering1.rotation = steering2.rotation;
	
	return steering1;
}

followPath.stopRadius = 5;

followPath.pathOffset = 60;