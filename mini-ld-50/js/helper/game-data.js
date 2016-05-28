"use strict";

/* Create and get all the images that are used */
var images = {};

createImages();

function createImages() {
	createImage('Rock');
	
	/* DuckManStuff */
	for(var i = 0; i <= 4; i++) {
		createImage('WalkUp'+i);
	}
	for(var i = 0; i <= 3; i++) {
		createImage('WalkRight'+i);
	}
	for(var i = 0; i <= 4; i++) {
		createImage('WalkDown'+i);
	}
	for(var i = 0; i <= 3; i++) {
		createImage('WalkLeft'+i);
	}
	for(var i = 1; i <= 5; i++) {
		createImage('AttackRight'+i);
	}
	for(var i = 1; i <= 5; i++) {
		createImage('AttackLeft'+i);
	}
	
	createImage('EnemyUp');
	createImage('EnemyRight');
	createImage('EnemyDown');
	createImage('EnemyLeft');
	for(var i = 1; i <= 3; i++) {
		createImage('EnemyDeathSequence'+i);
	}
}

function createImage(name) {
	images[name] = new Image();
	images[name].onload = function() {
		this.halfWidth = Math.floor(this.width/2);
		this.halfHeight = Math.floor(this.height/2);
	};
	images[name].src = 'images/' + name + '.png';
}

var tiles = [];
createTile('CharredTexture');
createTile('lavaTexture');
createTile('LeafTexture');
createTile('WaterTexture');

function createTile(name) {
	var i = tiles.length;
	tiles[i] = new Image();
	tiles[i].onload = function() {
		this.halfWidth = Math.floor(this.width/2);
		this.halfHeight = Math.floor(this.height/2);
	};
	tiles[i].src = 'images/' + name + '.png';
}

function areTilesReady() {
	var complete = true;
	
	for(var i in tiles) {
		complete = complete && tiles[i].complete;
	}
	
	return complete;
}


/* Game Object */
function GameObject(position, orientation) {
	this.position = (position) ? position : [0, 0, 0];
	this.orientation = (orientation) ? orientation : 0;
}

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

/* DuckMan game object */
function DuckMan(position, orientation) {
	GameObject.call(this, position, orientation);
	this.lookDirection = -1;	/* Set by the server */
	this.moving = false;	/* Set by the server */
	this.attacking = false;	/* Set by the server */
	this.death = 0;	/* Set by the server */
	this.deathTime = -1;
	this.atkTime = -1;
}
inheritPrototype(DuckMan, GameObject);

DuckMan.prototype.imgs = [];

setUpDuckManImages();
function setUpDuckManImages() {
	for(var i = 0; i < 9; i++) {
		DuckMan.prototype.imgs[i] = [];
	}
	
	for(var i = 0; i <= 4; i++) {
		DuckMan.prototype.imgs[0].push(images['WalkUp'+i]);
	}
	for(var i = 0; i <= 3; i++) {
		DuckMan.prototype.imgs[1].push(images['WalkRight'+i]);
	}
	for(var i = 0; i <= 4; i++) {
		DuckMan.prototype.imgs[2].push(images['WalkDown'+i]);
	}
	for(var i = 0; i <= 3; i++) {
		DuckMan.prototype.imgs[3].push(images['WalkLeft'+i]);
	}
	for(var i = 1; i <= 5; i++) {
		DuckMan.prototype.imgs[5].push(images['AttackRight'+i]);
	}
	for(var i = 1; i <= 5; i++) {
		DuckMan.prototype.imgs[7].push(images['AttackLeft'+i]);
	}
}

DuckMan.prototype.draw = function() {
	context.save();
	
	context.translate(this.position[0], this.position[2]);
	context.rotate(-1*this.orientation);	/* Multiple by -1 so its counter clockwise */
	
	var img;
	
	if(this.death > 0) {
		var n = this.imgs[8].length;
		
		if(this.death == 1) {
			if(this.deathTime == -1) {
				this.deathTime = Date.now();
			}
			
			var t = Date.now() - this.deathTime;
						
			if(t < n*100) {
				var i = Math.floor((t/100)%n);
				img = this.imgs[8][i];
			} else {
				img = this.imgs[8][n-1];
			}
		} else {
			img = this.imgs[8][n-1];
		}
	} else if(this.attacking && (this.lookDirection == 1 || this.lookDirection == 3)) {
		if(this.atkTime == -1) {
			this.atkTime = Date.now();
		}
		
		var t = Date.now() - this.atkTime;
		var n = this.imgs[this.lookDirection+4].length;
		
		var i = Math.floor((t/50)%n);
		img = this.imgs[this.lookDirection+4][i];
	} else {
		this.deathTime = -1;
		this.atkTime = -1;
		
		if(this.moving) {
			var t = Date.now();
			var n = this.imgs[this.lookDirection].length-1;
			
			var i;
			if(this.lookDirection == 0 || this.lookDirection == 2) {
				i = Math.floor((t/150)%n)+1;
			} else {
				i = Math.floor((t/120)%n)+1;
			}
			img = this.imgs[this.lookDirection][i];
		} else {
			img = this.imgs[this.lookDirection][0];
		}
	}
	
	if(img != null && img.complete) {
		context.drawImage(img, -img.halfWidth, -img.halfHeight, img.width, img.height);
	}
	
	context.restore();
};

/* Enemy */
function Enemy(position, orientation) {
	DuckMan.call(this, position, orientation);
}
inheritPrototype(Enemy, DuckMan);

Enemy.prototype.imgs = [];

setUpEnemyImages();
function setUpEnemyImages() {
	for(var i = 0; i < 9; i++) {
		Enemy.prototype.imgs[i] = [];
	}
	
	Enemy.prototype.imgs[0][0] = images['EnemyUp'];
	Enemy.prototype.imgs[1][0] = images['EnemyRight'];
	Enemy.prototype.imgs[2][0] = images['EnemyDown'];
	Enemy.prototype.imgs[3][0] = images['EnemyLeft'];
	
	for(var i = 1; i <= 3; i++) {
		Enemy.prototype.imgs[8].push(images['EnemyDeathSequence'+i]);
	}
}
