function Player(position) {
	this.position = (position) ? position : [0, 0, 0];
	this.orientation = 0;
	this.velocity = [0,0,0];
	this.maxSpeed = 250;
	this.lookDirection = 2;
	this.moving = false;
	this.atkTime = -1;
	this.health = 3;
	this.deathTime = -1;
}

Player.prototype.getCollisionBox = function() {
	var box = [];
	box[0] = this.position[0] - 22;
	box[1] = this.position[2] + 28;
	box[2] = 44;
	box[3] = 64;
	return box;
};

Player.prototype.attack = function() {
	var x, y, w, h;
	
	if(this.lookDirection == 1) {
		w = 185;
		h = 155;
		x = this.position[0];
		y = this.position[2]-110;
	} else if(this.lookDirection == 3) {
		w = 185;
		h = 185;
		x = this.position[0]-w;
		y = this.position[2]-110;
	}
	
	/* Remove this if when up and down attack are implemented */
	if(typeof(x) != "undefined") {
		for(var i in enemies) {
			if(enemies.health <= 0) {
				continue;
			}
		
			var enemyHitBox = enemies[i].getHitBox();
			if(doBoxesIntersect(x, y, w, h, enemyHitBox[0], enemyHitBox[1], enemyHitBox[2], enemyHitBox[3])) {
				enemies[i].takeDamage(1);
			}
		}
	}
};

/* Enemy */
function Enemy(position) {
	Player.call(this, position);
	this.maxSpeed = 150;
}
inheritPrototype(Enemy, Player);

Enemy.prototype.getHitBox = function() {
	var w = 62;
	var h = 190;
	
	return [this.position[0] - (w/2), this.position[2]-(h/2), w, h];
};

Enemy.prototype.takeDamage = function(num) {
	this.health -= num;
	
	if(this.deathTime == -1 && this.health <= 0) {
		this.deathTime = Date.now();
	}
};


var lastEnemyStep = Date.now();
function stepEnemies() {	
	var now = Date.now();
	var dt = (now - lastEnemyStep)/1000;

	for(var i in enemies) {
		if(enemies[i].health <= 0) {
			continue;
		}
	
		var player = findClosestPlayer(enemies[i].position);
		
		if(player) {
			var vel = arrive(enemies[i], player, dt);
			
			if(vel) {
				vec3.add(enemies[i].position, enemies[i].position, vel);
				
				if(isColliding(enemies[i])) {
					vec3.sub(enemies[i].position, enemies[i].position, vel);
				}
			}
		}
	}
	
	lastEnemyStep = now;
}


var canSeeRadius = 450;

function findClosestPlayer(point) {
	var closest = Infinity;
	var player = null;

	for(var i in players) {
		var dist = vec3.dist(point, players[i].position);
		if(dist < closest) {
			closest = dist;
			player = players[i];
		}
	}
	
	if(closest < canSeeRadius) {
		return player;
	} else {
		return null;
	}
}

/* The satisfaction radius */
var radius = 60;

/* Returns the steering for a character so it arrives at the target */
function arrive(character, target, dt) {
	/* Get the right direction for the velocity */
	var velocity = vec3.sub([], target.position, character.position);
	
	/* If we are close enough to the target return no steering */
	var dist = vec3.len(velocity);
	if(dist < radius || dist > canSeeRadius)  {
		return null;
	}
	
	vec3.normalize(velocity, velocity);
	vec3.scale(velocity, velocity, character.maxSpeed*dt);
	
	return velocity;
}

function playerUpdate(player, moveDirection, attack, dt) {
	if(attack && player.atkTime == -1 && (player.lookDirection == 1 || player.lookDirection == 3)) {
		player.atkTime = Date.now();
		player.attack();
	}
	
	if(player.atkTime > 0) {
		if((Date.now() - player.atkTime) > 250) {
			player.atkTime = -1;
		}
	} else {
	
		if(moveDirection[0] < 0) {
			player.lookDirection = 3;
		} else if(moveDirection[0] > 0) {
			player.lookDirection = 1;
		} else {
			if(moveDirection[2] < 0) {
				player.lookDirection = 0;
			} else if(moveDirection[2] > 0) {
				player.lookDirection = 2;
			}
		}
		
		if(moveDirection[0] != 0 || moveDirection[2] != 0) {
			player.moving = true;
		} else {
			player.moving = false;
		}

		vec3.set(player.velocity, 0, 0, 0);
		vec3.normalize(player.velocity, moveDirection);
		vec3.scale(player.velocity, player.velocity, player.maxSpeed*dt);
		vec3.add(player.position, player.position, player.velocity);
		
		if(isColliding(player)) {
			vec3.sub(player.position, player.position, player.velocity);
		}
		
	}
}

function isColliding(unit) {
	for(var sectionR in map) {
		for(var sectionC in map[sectionR]) {
			var x = -1*(mapWidth/2) + sectionC*sectionWidth;
			var y = -1*(mapHeight/2) + sectionR*sectionHeight;
			
			var playerBox = unit.getCollisionBox();
			
			if(doBoxesIntersect(playerBox[0], playerBox[1], playerBox[2], playerBox[3], x, y, sectionWidth, sectionHeight)) {
				//console.log("Player in Section " + sectionR + ", " + sectionC);
				if(collidesInSection(playerBox[0], playerBox[1], playerBox[2], playerBox[3], map[sectionR][sectionC], x, y)) {
					return true;
				}
			}
		}
	}
	
	return false;
}

function doBoxesIntersect(aX, aY, aW, aH, bX, bY, bW, bH) {
	return !( aX > (bX + bW)
		|| (aX+aW) < bX
		|| aY > (bY + bH)
		|| (aY+aH) < bY );
}

function collidesInSection(aX, aY, aW, aH, s, sX, sY) {
	for(var w in sectionWalls[s]) {
		var x = sX + sectionWalls[s][w].col*tileSize;
		var y = sY + sectionWalls[s][w].row*tileSize;
		
		if(doBoxesIntersect(aX, aY, aW, aH, x, y, tileSize, tileSize)) {
			//console.log("colliding with: " + sectionWalls[s][w].row + ", " + sectionWalls[s][w].col);
			return true;
		}
	}
	
	return false;
}