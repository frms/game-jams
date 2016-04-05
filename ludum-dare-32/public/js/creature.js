function Creature(id, x, y) {
	// The player and its settings
	var c = game.add.sprite(x, y, 'dude');
	c.gameID = id;
	c.anchor.set(0.5);
	c.speed = 150;
	c.moving = false;
	c.beginStopTime = -1;
	c.target = c.position.clone();

	//  Our two animations, walking left and right.
	c.animations.add('right', [12, 13, 14, 15], 8, true);
	c.animations.add('left', [4, 5, 6, 7], 8, true);
	c.animations.add('up', [8, 9, 10, 11], 8, true);
	c.animations.add('down', [0, 1, 2, 3], 8, true);


	c.move = move;
	c.moveToTarget = moveToTarget;
	c.setTarget = setTarget;

	overworldGroup.add(c);

	return c;
}

function move(now, dt) {
	this.moveToTarget(now, dt);

	if(this.position.equals(this.target)) {
		this.moving = false;
	}
}

function moveToTarget(now, dt) {
	// Evidently phaser can give us a dt of 0
	if(dt === 0) {
		return;
	}

	var vel = Phaser.Point.subtract(this.target, this.position);
	var dist = vel.getMagnitude();
	
	if(dist > this.speed*dt) {
		vel.normalize();

		vel.multiply(this.speed*dt, this.speed*dt);
	}

	if (vel.x < 0) {
		this.animations.play('left');
		this.beginStopTime = -1;
	}
	else if (vel.x > 0) {
		this.animations.play('right');
		this.beginStopTime = -1;
	} else if (vel.y < 0) {
		this.animations.play('up');
		this.beginStopTime = -1;
	}
	else if (vel.y > 0) {
		this.animations.play('down');
		this.beginStopTime = -1;
	}
	else {
		//  Stand still
		this.animations.stop();

		if(this.beginStopTime === -1) {
			this.beginStopTime = now;
		}

		if(now - this.beginStopTime > 500) {
			this.frame = this.animations.currentAnim._frames[0];
		}
	}

	this.x += Math.round(vel.x);
	this.y += Math.round(vel.y);
}

function isLocationOpen(x, y) {
	return x >= 0 && y >= 0 && x < map.width && y < map.height && map.getTile(x, y, blockedLayer) === null;
}



function Player(id, x, y) {
	var p = new Creature(id, x, y);

	p.move = playerMove;
	p.setTarget = setTarget;
	p.basicDmg = 35;

	return p;
}

function playerMove(now, dt) {
	move.call(this, now, dt);

	if(!this.moving) {
		var x = blockedLayer.getTileX(this.position.x);
		var y = blockedLayer.getTileY(this.position.y);

		if (cursors.left.isDown && isLocationOpen(x-1, y)) {
			this.setTarget(x-1, y);
		} else if (cursors.right.isDown && isLocationOpen(x+1, y)) {
			this.setTarget(x+1, y);
		} else if (cursors.up.isDown && isLocationOpen(x, y-1)) {
			this.setTarget(x, y-1);
		} else if (cursors.down.isDown && isLocationOpen(x, y+1)) {
			this.setTarget(x, y+1);
		}
	}
}

function setTarget(x, y) {
	this.target.x = (x + 0.5) * map.tileWidth;
	this.target.y = (y + 0.5) * map.tileWidth;
	this.moving = true;
	ws.send(JSON.stringify({
		move: [this.target.x, this.target.y]
	}));

	if(map.getTile(x, y, backgroundlayer) !== null) {

		if(Math.random() < 0.15) {
			getInThere(x, y);
			return;
		}
	}

	for(var i in battles) {
		var arr = i.split(' ');
		var x2 = Number.parseInt(arr[0]);
		var y2 = Number.parseInt(arr[1]);

		if(x2 === x && y2 === y) {
			getInThere(x2, y2);
		}
	}
}

function getInThere(x, y) {
	var posStr = x + ' ' + y;

	ws.send(JSON.stringify({
		joinBattle: posStr
	}));
}



function OtherPlayer(id, x, y) {
	var p = new Creature(id, x, y);

	p.targets = [];
	p.addTarget = addTarget;
	p.move = otherPlayerMove;

	return p;
}

function addTarget(t) {
	var lastTarget = this.target;

	if(this.targets.length > 0) {
		lastTarget = this.targets[this.targets.length-1];
	}

	if(!lastTarget.equals(t)) {
		this.targets.push(t);
	}
}

function otherPlayerMove(now, dt) {
	move.call(this, now, dt);

	if(!this.moving && this.targets.length > 0) {
		this.target = this.targets.splice(0, 1)[0];
		this.moving = true;
	}
}