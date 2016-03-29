"use strict";

function CollisionDetector(obstacles) {
	this.obstacles = (obstacles) ? obstacles : [];
}

/* This function takes in a ray with a starting point p and a ray vector r and finds
 the closest collision that the ray has with the list of obstacles */
CollisionDetector.prototype.getCollision = function(p, r) {
	/* Holds the closest collision */
	var closestCollision = null;
	
	/* Find the closest collision */
	for(var i = 0; i < this.obstacles.length; i++) {
		var collision = this.obstacles[i].getCollision(p, r);
		
		if(collision && (!closestCollision || collision.distance < closestCollision.distance)) {
			closestCollision = collision;
		}
	}
	
	return closestCollision;
}