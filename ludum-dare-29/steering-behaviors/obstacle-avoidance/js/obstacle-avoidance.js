"use strict";

/* Returns the steering for single ray obstacle avoidance. Will often clip corners,
 because of only using a single ray for detection. */
function obstacleAvoidance(character) {
	/* Creates the ray vector */
	var rayVector = vec3.clone(character.velocity);
	vec3.normalize(rayVector, rayVector);
	vec3.scale(rayVector, rayVector, obstacleAvoidance.lookahead);
	
	var collision = collisionDetector.getCollision(character.position, rayVector);
	
	/* If no collision do nothing */
	if(!collision) {
		return { linear: [0,0,0], angular:0 };
	}
	
	/* Create a target away from the wall to seek */
	vec3.scale(collision.normal, collision.normal, obstacleAvoidance.avoidDistance);
	vec3.add(collision.position, collision.position, collision.normal);
	
	/* If velocity and the collision normal are parallel then move the target a bit to
	 the left or right of the normal */
	if(vec3.len(vec3.cross([], character.velocity, collision.normal)) < 0.005) {
		vec3.add(collision.position, collision.position, [-collision.normal[2], collision.normal[1], collision.normal[0]]);
	}
	
	/* Call seek with a high acceleration so the character quickly changes velocity to go towards
	 the avoidance target. If I had collision detection between the characters and the walls that
	 prevented either from passing through each other then I would not have to use such a high
	 acceleration */
	return seek(character, new GameObject(collision.position), 700);
}

/* How far ahead the ray should extend */
obstacleAvoidance.lookahead = 100;

/* The distance away from the collision that we wish go */
obstacleAvoidance.avoidDistance = 50;