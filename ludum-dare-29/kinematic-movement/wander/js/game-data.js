"use strict";

/* Create and get all the images that are used */
var images = {};
createImage('trapezoid');
createImage('circle');

function createImage(name) {
	images[name] = new Image();
	images[name].onload = function() {
		this.halfWidth = Math.floor(this.width/2);
		this.halfHeight = Math.floor(this.height/2);
	};
	images[name].src = 'images/' + name + '.png';
}


/* Game Object */
function GameObject(position, orientation) {
	this.position = (position) ? position : [0, 0, 0];
	this.orientation = (orientation) ? orientation : 0;
}

GameObject.prototype.maxSpeed = 100;
GameObject.prototype.img = null;
GameObject.prototype.draw = function() {
	context.save();
	
	context.translate(this.position[0], this.position[2]);
	context.rotate(-1*this.orientation);	/* Multiple by -1 so its counter clockwise */
	
	if(this.img.complete) {
		context.drawImage(this.img, -this.img.halfWidth, -this.img.halfHeight, this.img.width, this.img.height);
	}
	
	context.restore();
};

/* Trapezoid game object */
function Trapezoid(position, orientation) {
	GameObject.call(this, position, orientation);
}
inheritPrototype(Trapezoid, GameObject);

Trapezoid.prototype.img = images['trapezoid'];

/* Circle game object */
function Circle(position, orientation) {
	GameObject.call(this, position, orientation);
}
inheritPrototype(Circle, GameObject);

Circle.prototype.img = images['circle'];
