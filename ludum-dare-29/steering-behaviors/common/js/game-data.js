"use strict";

/* Create and get all the images that are used */
var images = {};
createImage('trapezoid');
createImage('circle');
createImage('pointedCircle');
createImage('player1');
createImage('player2');
createImage('player3');
createImage('player4');
createImage('rock');
createImage('mouse1');
createImage('mouse2');
createImage('mouse3');
createImage('mouse4');
createImage('enemy1');
createImage('enemy2');
createImage('enemy3');
createImage('enemy4');
createImage('attack05');
createImage('soundOn');
createImage('soundOff');
createImage('help');
createImage('helpScreen');
createImage('refresh');

function createImage(name) {
	images[name] = new Image();
	images[name].onload = function() {
		this.halfWidth = Math.floor(this.width/2);
		this.halfHeight = Math.floor(this.height/2);
	};
	images[name].src = 'steering-behaviors/common/images/' + name + '.png';
}


/* Game Object */
function GameObject(position, orientation) {
	this.position = (position) ? position : [0, 0, 0];
	this.orientation = (orientation) ? orientation : 0;
	this.velocity = [0, 0, 0];
	this.rotation = 0;
    
    this.img = this.imgs[0][0];
}

GameObject.prototype.maxSpeed = 150;
GameObject.prototype.maxRotation = 1.5*Math.PI;
GameObject.prototype.imgs = null;

GameObject.prototype.update = function(dt, steering) {
	/* Update the position and orientation */
	var temp = vec3.scale([], this.velocity, dt);
	vec3.add(this.position, this.position, temp);
	this.orientation += this.rotation * dt;
	
	/* Update the velocity and rotation */
	if(steering) {
		if(typeof steering.velocity != "undefined") {
			this.velocity = steering.velocity;
		} else {
			temp = vec3.scale([], steering.linear, dt);
			vec3.add(this.velocity, this.velocity, temp);
		}
		
		if(typeof steering.rotation != "undefined") {
			this.rotation = steering.rotation;
		} else {
			this.rotation += steering.angular * dt;
		}
	}
	
	/* Check to see if we are going too fast */
	if(vec3.len(this.velocity) > this.maxSpeed) {
		vec3.normalize(this.velocity, this.velocity);
		vec3.scale(this.velocity, this.velocity, this.maxSpeed);
	}
	
	/* Check to see if we are rotating too fast */
	var rotationSize = Math.abs(this.rotation);
	if(rotationSize > this.maxRotation) {
		this.rotation /= rotationSize;
		this.rotation *= this.maxRotation;
	}
};

GameObject.prototype.draw = function() {
	if(this.path) {
		this.path.draw();
	}

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

/**
 * Circle game object
 * This game object will always move in the direction of its moveDirection 
 * vector at max speed.
 */
function Circle(position, orientation) {
	GameObject.call(this, position, orientation);
	this.moveDirection = [0, 0, 0];
}
inheritPrototype(Circle, GameObject);

Circle.prototype.maxSpeed = 100;
Circle.prototype.img = images['circle'];

/* Set the circle's velocity to its move direction vector with masSpeed before updating like normal*/
Circle.prototype.update = function(dt, steering) {
	vec3.set(this.velocity, 0, 0, 0);
	vec3.normalize(this.velocity, this.moveDirection);
	vec3.scale(this.velocity, this.velocity, this.maxSpeed);
	
	GameObject.prototype.update.call(this, dt, steering);
};

/* PointedCircle game object */
function PointedCircle(position, orientation) {
	GameObject.call(this, position, orientation);
    
    this.goScore = false;
    this.toBeRemoved = false;
    this.killed = false;
    
    this.img = this.imgs[0][1];
}
inheritPrototype(PointedCircle, GameObject);

PointedCircle.prototype.imgs = [ [ images['mouse1'], images['mouse2'], images['mouse3'],images['mouse4'] ] ];

PointedCircle.prototype.draw = function() {
	if(this.path) {
		this.path.draw();
	}

	context.save();
	
	context.translate(this.position[0], this.position[2]);
	context.rotate(-1*this.orientation);	/* Multiple by -1 so its counter clockwise */
    
	if(this.img.complete) {
		context.drawImage(this.img, -this.img.halfWidth, -29, this.img.width, this.img.height);
	}
	
	context.restore();
};

PointedCircle.prototype.update = function(dt, gameObjects, obstacles) {
    var steering;

    /* If the guy gets far enough to the left then go score, else flock */
    if(this.goScore || this.position[0] < (-1/2)*screenWidth+100) {
        steering = seekGoal(this, gameObjects, obstacles);
        this.goScore = true;
    } else {
        steering = flocking(this, gameObjects, obstacles);
    }
    
    /* If moving then animate */
    if(vec3.len(this.velocity)) {
        var t = Date.now();
        var n = this.imgs[0].length-1;
        
        var i = Math.floor((t/200)%n)+1;
        this.img = this.imgs[0][i];
    }

    /* If off screen set it for removal */
    if( this.position[0] < (-1/2)*screenWidth-this.img.width || this.position[0] > (1/2)*screenWidth+this.img.width
        || this.position[2] < (-1/2)*screenHeight-this.img.height || this.position[2] > (1/2)*screenHeight+this.img.height ) {
        this.remove();
    }
    
    GameObject.prototype.update.call(this, dt, steering);
};

PointedCircle.prototype.remove = removeGameObject;

function removeGameObject() {
    if( !this.toBeRemoved ) {
        removalList.push(this);
        this.toBeRemoved = true;
    }
};

/* Seeks the goal towards the left of the screen */
function seekGoal(character, gameObjects, obstacles) {
    var seekSteering = seek(character, scoreTarget);
    
    var separationSteering = separation(character, gameObjects, obstacles);
    
    var steering = blendSteering(seekSteering, 1, separationSteering, 2);
    
    var lookWhereGoingSteering = lookWhereYoureGoing(character);
    
    steering.angular = lookWhereGoingSteering.angular;
    steering.rotation = lookWhereGoingSteering.rotation;
    
    return steering;
}

/* Player game object */
function Player(position, orientation) {
	GameObject.call(this, position, orientation);
    this.moveDirection = [0, 0, 0];
    this.attackObj = null;
    this.attackStartTime = -1;
}
inheritPrototype(Player, GameObject);

Player.prototype.maxSpeed = 150;
Player.prototype.maxRotation = 2.2*Math.PI;
Player.prototype.imgs = [ [ images['player1'], images['player2'], images['player3'],images['player4'] ] ];

Player.prototype.draw = function() {
	if(this.path) {
		this.path.draw();
	}

	context.save();
	
	context.translate(this.position[0], this.position[2]);
	context.rotate(-1*this.orientation);	/* Multiple by -1 so its counter clockwise */
    
	if(this.img.complete) {
		context.drawImage(this.img, -this.img.halfWidth, -29, this.img.width, this.img.height);
	}
    
    if(this.attackObj) {
        context.save();
        context.translate(0, 40);
        this.attackObj.draw();
        context.restore();
    }
	
	context.restore();
};

/* Set the players's velocity to its move direction vector with masSpeed before updating like normal. Don't move if we collide with an obstacle. */
Player.prototype.update = function(dt, steering, obstacles) {
    if(this.attackObj) {
        this.attackObj.update();
        
        /* Attack any rats in front of me */
        for(var i in gameObjects) {
            if(gameObjects[i] instanceof Enemy && isInFront(this, gameObjects[i])
                && vec3.dist(this.position, gameObjects[i].position) < 60) {
                gameObjects[i].remove();
            }
        }
        
        if( (Date.now() - this.attackStartTime) > 400) {
            this.attackObj = null;
            this.attackStartTime = -1;
        }
    }

	vec3.set(this.velocity, 0, 0, 0);
	vec3.normalize(this.velocity, this.moveDirection);
	vec3.scale(this.velocity, this.velocity, this.maxSpeed);
	
    var prevPosition = vec3.clone(this.position);
    
	GameObject.prototype.update.call(this, dt, steering);
    
    /* If at the edges of the screen then stop moving */
    if( this.position[0] < (-1/2)*screenWidth+this.img.halfWidth || this.position[0] > (1/2)*screenWidth-this.img.halfWidth
        || this.position[2] < (-1/2)*screenHeight+this.img.halfHeight || this.position[2] > (1/2)*screenHeight-this.img.halfHeight ) {
        this.position = prevPosition;
    } else {
        /* Collision detection */
        for(var i in obstacles) {            
            if(this.collidesWithObstacle(obstacles[i])) {
                /* Stop moving */
                this.position = prevPosition;
                break;
            }
        }
    }
    
    /* If moving then animate */
    if(vec3.len(this.velocity)) {
        var t = Date.now();
        var n = this.imgs[0].length-1;
        
        var i = Math.floor((t/200)%n)+1;
        this.img = this.imgs[0][i];
    }
};

Player.prototype.collidesWithObstacle = function(obstacle) {
    var collideDist = (obstacle.halfSize + 15)*0.8;
    
    return (vec3.dist(this.position, obstacle.position) < collideDist);
}

Player.prototype.attack = function(gameObjects) {
    if(!this.attackObj) {
        if(soundOn) {
            $('#attackSound')[0].play();
        }
        this.attackObj = new Attack();
        this.attackStartTime = Date.now();
    }
};

/* Rock game object */
function Rock(position, orientation, size) {
    if(!position) {
        var x = Math.random()*screenWidth - screenWidth/2;
        var z = Math.random()*screenHeight - screenHeight/2;
        position = [x, 0, z];
    }   
    
	GameObject.call(this, position, orientation);
    
    if(size) {
        this.size = size;
    } else {
        this.size = Math.random()*50 + 20;
    }
    this.halfSize = this.size/2;
}
inheritPrototype(Rock, GameObject);

Rock.prototype.imgs = [ [ images['rock'] ] ];

Rock.prototype.draw = function() {
	context.save();
	
	context.translate(this.position[0], this.position[2]);
	context.rotate(-1*this.orientation);	/* Multiple by -1 so its counter clockwise */
	
	if(this.img.complete) {
		context.drawImage(this.img, -this.halfSize, -this.halfSize, this.size, this.size);
	}
	
	context.restore();
};

/* Enemy game object */
function Enemy(position, orientation) {
	GameObject.call(this, position, orientation);
    
    this.toBeRemoved = false;
    this.attackObj = null;
    this.attackStartTime = -1;
}
inheritPrototype(Enemy, GameObject);

Enemy.prototype.imgs =  [ [ images['enemy1'], images['enemy2'], images['enemy3'],images['enemy4'] ] ];

Enemy.prototype.draw = function() {
	if(this.path) {
		this.path.draw();
	}

	context.save();
	
	context.translate(this.position[0], this.position[2]);
	context.rotate(-1*this.orientation);	/* Multiple by -1 so its counter clockwise */
    
	if(this.img.complete) {
		context.drawImage(this.img, -this.img.halfWidth, -29, this.img.width, this.img.height);
	}
    
    if(this.attackObj) {
        context.save();
        context.translate(0, 40);
        context.scale(0.9,0.9);
        this.attackObj.draw();
        context.restore();
    }
	
	context.restore();
};

Enemy.prototype.update = function(dt, gameObjects, obstacles) {
    var steering = flocking(this, gameObjects, obstacles);
    
    /* Attack any rats in front of me */
    for(var i in gameObjects) {
        if(gameObjects[i] instanceof PointedCircle && isInFront(this, gameObjects[i])
            && vec3.dist(this.position, gameObjects[i].position) < 40) {
            gameObjects[i].remove();
            gameObjects[i].killed = true;
            
            this.attackObj = new Attack();
            this.attackStartTime = Date.now();
        }
    }
    
    if(this.attackObj) {
        this.attackObj.update(-1);
        
        if( (Date.now() - this.attackStartTime) > 400) {
            this.attackObj = null;
            this.attackStartTime = -1;
        }
    }
    
    /* If moving then animate */
    if(vec3.len(this.velocity)) {
        var t = Date.now();
        var n = this.imgs[0].length-1;
        
        var i = Math.floor((t/200)%n)+1;
        this.img = this.imgs[0][i];
    }

    /* If off screen set it or removal */
    if( !this.toBeRemoved && (this.position[0] < (-1/2)*screenWidth-this.img.width || this.position[0] > (1/2)*screenWidth+this.img.width
        || this.position[2] < (-1/2)*screenHeight-this.img.height || this.position[2] > (1/2)*screenHeight+this.img.height) ) {
        removalList.push(this);
        this.toBeRemoved = true;
    }
    
    GameObject.prototype.update.call(this, dt, steering);
}

Enemy.prototype.remove = removeGameObject;


/* Attack game object */
function Attack(position, orientation) {
	GameObject.call(this, position, orientation);
}
inheritPrototype(Attack, GameObject);

Attack.prototype.update = function(direction) {
    var d = (direction) ? direction : 1;
    this.orientation = d*(Date.now()%360)*Math.PI/360;
};

Attack.prototype.imgs = [ [ images['attack05'] ] ];


/* Button game object */
function Button(position, orientation, size) {
	GameObject.call(this, position, orientation);
}
inheritPrototype(Button, GameObject);

Button.prototype.imgs = [ 
    [ images['soundOff'], images['soundOn'] ],
    [ images['help'] ],
    [ images['helpScreen'] ],
    [ images['refresh'] ]
];

Button.prototype.isInImage = function(x,y) {
    var left = this.position[0] - this.img.halfWidth;
    var right = this.position[0] + this.img.halfWidth;
    var top = this.position[2] - this.img.halfHeight;
    var bottom = this.position[2] + this.img.halfHeight;
    
    if(window.innerWidth == screen.width && window.innerHeight == screen.height) {
        var widthMult = screen.width / screenWidth;
        var heightMult = screen.height / screenHeight;
        
        left *= widthMult;
        right *= widthMult;
        top *= heightMult;
        bottom *= heightMult;
    }
    
    return (x >= left && x <= right && y >= top && y <= bottom);
};