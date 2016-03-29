"use strict";

var screenWidth;
var screenHeight;

var context;
var fps;

var camera;

var obstacles = [];
var gameObjects = [ new Player(), new PointedCircle([100, 0, -30]), new PointedCircle([250, 0, 30]), new PointedCircle([350, 0, 150]) ];
//var gameObjects = [ new Player(), new Enemy([100, 0, -30]) ];
var character = gameObjects[0];
var scoreTarget;

var soundBtn = new Button([955, 0, 30]);
var helpBtn = new Button([990, 0, 30]);
helpBtn.img = helpBtn.imgs[1][0];
var helpScreen = new Button([512, 0, 288]);
helpScreen.img = helpBtn.imgs[2][0];
var refreshBtn = new Button([920, 0, 30]);
refreshBtn.img = helpBtn.imgs[3][0];

var canvas;

$(document).ready(function() {
    $('#attackSound')[0].volume = 0.5;

	canvas = $("#gameCanvas")[0];
	
	screenWidth = canvas.width;
	screenHeight = canvas.height;
	
	context = canvas.getContext("2d");
	context.shadowColor = '#8B907C';
    context.fillStyle = '#F5F5F5';
    context.font="30px sans-serif";
	
	fps = $("#fps")[0];
	
	setKeyEvents();
    
    $("#gameCanvas").click(canvasClick);
	
	camera = new Camera();
    
    /* Create new obstacles */
    for(var i = 0; i < 40; i++) {
        var rock = new Rock();
        
        if(!character.collidesWithObstacle(rock)) {
            obstacles.push(rock);
        }
    }
    
    scoreTarget = { position: [(-1/2)*screenWidth-100, 0, 0]};
	
	requestAnimationFrame(gameLoop);
});


function setKeyEvents() {
    var keyUsed = {};

	$(document).keydown(function(e) {
        /* Make the player attack */
        if(e.which == 32 && !keyUsed[e.which]) {
            character.attack(gameObjects);
            console.log("space");
        }
    
        if(e.which == 70) {
            requestFullscreen(canvas);
        }
    
		var target = character;
		
		if(target) {
			if(e.which == 37 && target.moveDirection[0] >= 0) {
				target.moveDirection[0] = -1;
			} else if(e.which == 39 && target.moveDirection[0] <= 0) {
				target.moveDirection[0] = 1;
			} else if(e.which == 38 && target.moveDirection[2] >= 0) {
				target.moveDirection[2] = -1;
			} else if(e.which == 40 && target.moveDirection[2] <= 0) {
				target.moveDirection[2] = 1;
			}
		}
        
        keyUsed[e.which] = true;
	});
	
	$(document).keyup(function(e) {
		var target = character;
		
		if(target) {
			if(e.which == 37 && target.moveDirection[0] < 0) {
				target.moveDirection[0] = 0;
			} else if(e.which == 39 && target.moveDirection[0] > 0) {
				target.moveDirection[0] = 0;
			} else if(e.which == 38 && target.moveDirection[2] < 0) {
				target.moveDirection[2] = 0;
			} else if(e.which == 40 && target.moveDirection[2] > 0) {
				target.moveDirection[2] = 0;
			}
		}
        
        keyUsed[e.which] = false;
	});
}

var showHelp = false;
var refresh = false;

function canvasClick(e) {
    //console.log(e.offsetX + " " + e.offsetY);
    
    if(!showHelp) {
        if(soundBtn.isInImage(e.offsetX, e.offsetY)) {
            toggleSound();
        }
        
        if(helpBtn.isInImage(e.offsetX, e.offsetY)) {
            showHelp = true;
        }
        
        if(refreshBtn.isInImage(e.offsetX, e.offsetY)) {
            refresh = true;
        }
    } else {
        showHelp = false;
    }
}

var soundOn = false;

function toggleSound() {
    soundOn = !soundOn;
    
    if(soundOn) {
        soundBtn.img = soundBtn.imgs[0][1];
        $('#music')[0].play();
    } else {
        soundBtn.img = soundBtn.imgs[0][0];
        $('#music')[0].pause();
    }
}


var lastFrame = 0;

function gameLoop(timestamp) {
	updateFPS(timestamp);

    var dt = (timestamp - lastFrame) / 1000;
    dt = (dt > 1/15) ? 1/15 : dt; 
	lastFrame = timestamp;
    
    stepGame(dt, timestamp);
    drawGame();
	
	requestAnimationFrame(gameLoop);
};

var timeoutID;

timeoutID = setInterval(function(){
    if(showHelp) {
        return;
    }

    if(gameObjects.length < 100) {
        var x = (Math.random()*screenWidth) - 370;
        var z = (Math.random()*screenHeight) - 288;
        var orientation = Math.random()*Math.PI*2;
        
        if(Math.random() < 0.05) {
            gameObjects.push(new Enemy([x,0,z], orientation));
        } else {
            gameObjects.push(new PointedCircle([x,0,z], orientation));
        }
    }
}, 500);

var removalList = [];
var saved = 0;
var died = 0;
var lost = 0;

function stepGame(dt) {
    if(showHelp) {
        return;
    }
    
    if(refresh) {
        /* Create new obstacles */
        obstacles = [];
        
        for(var i = 0; i < 40; i++) {
            var rock = new Rock();
            
            if(!character.collidesWithObstacle(rock)) {
                obstacles.push(rock);
            }
        }
        
        refresh = false;
    }

    /* Remove game objects from the game */
    while(removalList.length > 0) {
        var popped = removalList.pop();
        
        var index = gameObjects.indexOf(popped);
        
        if(index >= 0) {
            gameObjects.splice(index, 1);
            
            if(popped.goScore) {
                saved++;
            } else if(popped.killed) {
                died++;
            } else if(popped instanceof PointedCircle) {
                lost++;
            }
        }
    }

    for(var i = 1; i < gameObjects.length; i++) {        
        gameObjects[i].update(dt, gameObjects, obstacles);
    }
    
    character.update(dt, lookWhereYoureGoing2(character, dt), obstacles);
    
    hudText = "Saved: " + saved + " Died: " + died + " Lost: " + lost;
}

var hudText = ""

function drawGame() {
	context.clearRect(0, 0, screenWidth, screenHeight);
	
	context.save();
	
	camera.applyTransforms();
	
	//drawCoordinateGrid();
	
	for(var i = 0; i < obstacles.length; i++) {
		obstacles[i].draw();
	}	

	for(var i = 0; i < gameObjects.length; i++) {
		gameObjects[i].draw();
	}
    
	context.restore();
    

    context.shadowBlur = 3;
    context.fillText(hudText,15,35);
    context.shadowBlur = 0;
    
    refreshBtn.draw();
    soundBtn.draw();
    helpBtn.draw();
    
    if(showHelp) {
        helpScreen.draw();
    }
}

