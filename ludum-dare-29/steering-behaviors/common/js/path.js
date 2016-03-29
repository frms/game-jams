"use strict";

/* This function creates a path of line segments */
function Path(nodes) {
	this.nodes = nodes;
	this.calcDistances();
}

/* Loops through the path's nodes and determines how far each node in the path is 
from the starting node */
Path.prototype.calcDistances = function() {
	this.distances = [];
	this.distances[0] = 0;
	
	for(var i = 0; i < this.nodes.length - 1; i++) {
		this.distances[i+1] = this.distances[i] + vec3.dist(this.nodes[i], this.nodes[i+1]);
	}
	
	this.maxDist = this.distances[this.distances.length-1];
};

/* Draws the path on the context */
Path.prototype.draw = function() {
	context.beginPath();
	
	context.moveTo(this.nodes[0][0], this.nodes[0][2]);
	for(var i = 1; i < this.nodes.length; i++) {
		context.lineTo(this.nodes[i][0], this.nodes[i][2]);
	}
	
	context.strokeStyle = "#0094FF";
	context.stroke();
};

/* Gets the param for the closest point on the path given a position */
Path.prototype.getParam = function(position, lastParam) {
	/* Find the first point in the closest line segment to the path */
	var closestDist = distToSegment(position, this.nodes[0], this.nodes[1]);
	var closestSegment = 0;
	
	for(var i = 1; i < this.nodes.length - 1; i++) {
		var dist = distToSegment(position, this.nodes[i], this.nodes[i+1]);
		
		if(dist < closestDist) {
			closestDist = dist;
			closestSegment = i;
		}
	}
	
	var param = this.distances[closestSegment] + getParamForSegment(position, this.nodes[closestSegment], this.nodes[closestSegment+1]);
	
	return param; 
}

/* Given a param it gets the position on the path */
Path.prototype.getPosition = function(param) {
	/* Find the first node that is farther than given param */
	for(var i = 0; i < this.distances.length; i++) {
		if(this.distances[i] > param) {
			break;
		}
	}
	
	/* Convert it to the first node of the line segment that the param is in */
	if (i < 0) {
		i = 0;
	} else if (i > this.distances.length - 2) {
		i = this.distances.length -2;
	} else {
		i -= 1;
	}
	
	/* Get how far along the line segment the param is */
	var t = (param - this.distances[i]) / vec3.dist(this.nodes[i], this.nodes[i+1]);
	
	/* Get the position of the param */
	return vec3.lerp([], this.nodes[i], this.nodes[i+1], t);
}

/* Gives the distance of a point to a line segment.
p is the point, v and w are the two points of the line segment */
function distToSegment(p, v, w) { 
	return Math.sqrt(distToSegmentSquared(p, v, w));
}

/* Gives the squared distance of a point to a line segment.
p is the point, v and w are the two points of the line segment */
function distToSegmentSquared(p, v, w) {
	var l2 = vec3.sqrDist(v, w);
	
	if (l2 == 0) {
		return vec3.sqrDist(p, v);
	}
	
	var t = ((p[0] - v[0]) * (w[0] - v[0]) + (p[2] - v[2]) * (w[2] - v[2])) / l2;
	
	if (t < 0) {
		return vec3.sqrDist(p, v);
	}
	
	if (t > 1) {
		return vec3.sqrDist(p, w);
	}
	
	var closestPoint = vec3.lerp([], v, w, t);
	
	return vec3.sqrDist(p, closestPoint);
}

/* Finds the param for the closest point on the segment vw given the point p */
function getParamForSegment(p, v, w) {
	var l2 = vec3.sqrDist(v, w);
	
	if (l2 == 0) {
		return 0;
	}
	
	var t = ((p[0] - v[0]) * (w[0] - v[0]) + (p[2] - v[2]) * (w[2] - v[2])) / l2;
	
	if(t < 0) {
		t = 0;
	} else if (t > 1) {
		t = 1;
	}
	
	return t * Math.sqrt(l2);
}