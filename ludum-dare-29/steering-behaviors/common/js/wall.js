"use strict";

/* This function creates a wall of line segments */
function Wall(nodes) {
	this.nodes = nodes;
}

/* Draws the wall on the context */
Wall.prototype.draw = function() {
	context.beginPath();
	
	context.moveTo(this.nodes[0][0], this.nodes[0][2]);
	for(var i = 1; i < this.nodes.length; i++) {
		context.lineTo(this.nodes[i][0], this.nodes[i][2]);
	}
	
	context.strokeStyle = "#4800FF";
	context.stroke();
};

/* This function takes in a ray with a starting point p and a ray vector r (holds 
 length and direction of the ray, so its not an infinite length ray). If the ray
 intersects the wall the function returns the collision position and normal */
Wall.prototype.getCollision = function(p, r) {
	/* Holds the closest distance to a collision */
	var closest = Infinity;
	/* Holds the collision (intersection point and normal) */
	var collision = null;
	
	for(var i = 0; i < this.nodes.length-1; i++) {
		var l1 = this.nodes[i];
		var l2 = this.nodes[i+1];
	
		/* If the ray and line segment were lines at what point would they intersect */
		var rayEnd = vec3.add([], p, r);
		var point = lineLineIntersection(p, rayEnd, l1, l2);
		
		/* Is that intersection point is on the ray and the line segment */
		if( point && isPointOnLineSegment(l1, l2, point) && isPointOnLineSegment(p, rayEnd, point) ) {
			/* If the point is the closer to the start of the ray than any previously found point set it as the new closest point */
			var dist = vec3.dist(p, point);
			if(dist < closest) {
				collision = {}
				collision.position = point;
				collision.normal = getLineSegmentNormal(l1, l2, p);
				collision.distance = dist;
				
				closest = dist;
			}
		}
	}
	
	return collision;
};

/* This function takes in two lines, where p1/p2 are points on line 1 and p3/p4 are points
 on line 2. If the two lines intersect it returns the point of intersection or null if they 
 do not */
function lineLineIntersection(p1, p2, p3, p4) {
	/* Convert the lines to Ax + Bz = C format */
	var A1 = p2[2] - p1[2];
	var B1 = p1[0] - p2[0];
	var C1 = A1*p1[0] + B1*p1[2];
	
	var A2 = p4[2] - p3[2];
	var B2 = p3[0] - p4[0];
	var C2 = A2*p3[0] + B2*p3[2];
	
	var det = A1*B2 - A2*B1;
	
	/* If the determinant is 0 then lines are parallel */
	if(det == 0) {
		return null;
	}
	/* Calculate the intersection point */
	else {
		var position = new Array(3);
		position[0] = (B2*C1 - B1*C2)/det;
		position[1] = 0;
		position[2] = (A1*C2 - A2*C1)/det;
		
		return position;
	}
}

/* Returns true if point is on the line segment l1 to l2 */
function isPointOnLineSegment(l1, l2, point) {
	var t = getTForPointOnLine(l1, l2, point);
	return t >= 0 && t <= 1;
}

/* This function takes in a line and a point on the line, where l1/l2 are two 
 points that make up the line and p is a point on the line. It returns a 
 value t such that l1 + (l2-l1)t = p. Assumes that p is on the line l1 l2. */
function getTForPointOnLine(l1, l2, p) {
	/* If its a vertical line then calculate t based on the z value */
	if( l2[0] - l1[0] == 0 ) {
		return (p[2] - l1[2]) / (l2[2] - l1[2]);
	}
	/* Else calculate t based on the x value */
	else {
		return (p[0] - l1[0]) / (l2[0] - l1[0]);
	}
}

/* This functions takes in a line segment and a point. It returns a normal to the 
 line segment that is on the same side of the line as the point */ 
function getLineSegmentNormal(l1, l2, p) {
	/* Get the direction of the line segment */
	var ret = vec3.sub([], l1, l2);
	
	/* Make it perpendicular and normalize */
	vec3.set(ret, -1*ret[2], ret[1], ret[0]);
	vec3.normalize(ret, ret);
	
	/* Make sure the normal is on the same side of the line as the given point */
	var temp = vec3.add([], l1, ret);
	if(getSideOfLine(l1, l2, temp) != getSideOfLine(l1, l2, p)) {
		vec3.scale(ret, ret, -1);
	}
	
	return ret;
}

/* Given a line an a point this function will return true or false depending on which side of the line p is on */
function getSideOfLine(l1, l2, p) {
	return (l2[0] - l1[0])*(p[2] - l1[2]) > (l2[2] - l1[2])*(p[0] - l1[0]);
}