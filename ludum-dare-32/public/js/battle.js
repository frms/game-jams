var battle = function(game){}

var battleGroup;

(function() {
var playerSprites = {};
var playerHealthSprites = {};
var enemySprites = [];
var text;

var atkAbilitySprite;
var fleeAbilitySprite;

var cursorH;
var cursorV;

var abilitiesStartX = 408;
var abilitiesStartY = 459;
var abiltiesDeltaX = 80;
var abilitiesSize = 60;

var fighterStartY = 87;
var fighterDeltaY = 110;
var fighterHealthStartY = 38;

var playersStartX = 100;
var enemyStartX = 796;

var enemyHalfWidth = (315*(80/410)) / 2;
var cursorVXOffset = -30;

var healthBarWidth = 50;

var maxHealth = 100;

var waitForKeyUp = true;
var firstFrame = true;
var magicAnton;

battle.prototype = {
	create: function(){
		game.world.setBounds(0, 0, game.width, game.height);

		if(!battleGroup) {
			battleGroup = game.add.group();

			var style = { font: "25px Arial", fill: "#ffffff", align: "center" };

			text = game.add.text(35, game.world.centerY, "you", style);

			text.anchor.set(0.5);

			battleGroup.add(text);

			atkAbilitySprite = game.add.sprite(abilitiesStartX, abilitiesStartY, 'basicAtk');
			atkAbilitySprite.anchor.set(0.5);
			battleGroup.add(atkAbilitySprite);

			fleeAbilitySprite = game.add.sprite(abilitiesStartX + abiltiesDeltaX, abilitiesStartY, 'flee');
			fleeAbilitySprite.anchor.set(0.5);
			battleGroup.add(fleeAbilitySprite);

			cursorH = game.add.sprite(abilitiesStartX, abilitiesStartY - abilitiesSize/2 - 10, 'cursorH');
			cursorH.anchor.set(0.5, 1);
			battleGroup.add(cursorH);

			cursorV = game.add.sprite(enemyStartX - enemyHalfWidth + cursorVXOffset, fighterStartY, 'cursorV');
			cursorV.anchor.set(1, 0.5);
			battleGroup.add(cursorH);

			player.abilities = [basicAtk, flee];
		}

		battleGroup.visible = true;
		cursorH.visible = true;
		cursorV.visible = false;

		player.x = player.target.x;
		player.y = player.target.y;

		waitForKeyUp = true;
		firstFrame = true
		magicAnton = timer.now() + 100;

		aKey.onDown.removeAll();
		aKey.onDown.add(this.aPressed, this);

		this.updateBattle();
	},

	createFighterSprite: function(str, x, y) {
		var result = game.add.sprite(x, y, str);
		var desiredScale = result.height/80;
		result.scale.x /= desiredScale;
		result.scale.y /= desiredScale;
		result.anchor.set(0.5);

		return result;
	},

	createHealthBarSprite: function(x, y) {
		var result = game.add.sprite(x - healthBarWidth/2, y, 'healthBar');
		result.anchor.set(0, 0.5);

		return result;
	},

	update: function() {
		fpsStats.end();

		fpsStats.begin();

		updateWorldState();

		var i = (cursorH.x - abilitiesStartX) / abiltiesDeltaX;

		if(cursorH.visible) {
			useCursorH(i);
		} else {
			// use ability
			player.abilities[i]();
		}
	},

	aPressed: function() {
		// ws.send(JSON.stringify({
		// 	atk: true
		// }));
	},

	shutdown: function() {
		for(var i in playerSprites) {
			playerSprites[i].destroy();
			playerHealthSprites[i].destroy();
		}

		playerSprites = {};
		playerHealthSprites = {};

		for(var i in enemySprites) {
			if(enemySprites[i]) {
				enemySprites[i].destroy();
			}
		}

		enemySprites = [];

		fleeSent = false;
	},

	updateBattle: function update() {
		this.updatePlayers();
		this.updateEnemies();
	},

	updatePlayers: function() {
		var players = battles[player.curBattle].players;

		for(var i in playerSprites) {
			if(players.indexOf(i) === -1) {
				playerSprites[i].destroy();
				delete playerSprites[i];
				playerHealthSprites[i].destroy();
				delete playerHealthSprites[i];
			}
		}

		for(var i = 0; i < players.length; i++) {
			var gameID = players[i];

			if(!playerSprites[gameID]) {
				playerSprites[gameID] = this.createFighterSprite('playerBattle', playersStartX, 0);
				playerHealthSprites[gameID] = this.createHealthBarSprite(playersStartX, 0);
			}
		}

		var ph = battles[player.curBattle].playersHealth;
		
		for(var i = 0; i < players.length; i++) {
			var gameID = players[i];

			playerSprites[gameID].position.y = fighterStartY + fighterDeltaY*i;
			playerHealthSprites[gameID].position.y = fighterHealthStartY + fighterDeltaY*i;

			if(ph) {
				playerHealthSprites[gameID].scale.x = clamp(ph[gameID] / maxHealth, 0, 1);
			}

			if(gameID === player.gameID) {
				text.position.y = 117 + fighterDeltaY*i;
			}
		}
	},

	updateEnemies: function() {
		var enemies = battles[player.curBattle].enemies;

		for(var i = 0; i < enemies.length; i++) {
			var realI = i*2;

			if(enemies[i]) {
				if(!enemySprites[realI]) {
					enemySprites[realI] = this.createFighterSprite('enemyBattle', enemyStartX, fighterStartY + fighterDeltaY*i);
					enemySprites[realI+1] = this.createHealthBarSprite(enemyStartX, fighterHealthStartY + fighterDeltaY*i);
				}

				enemySprites[realI+1].scale.x = clamp(enemies[i].health / maxHealth, 0, 1);
			} else {
				if(enemySprites[realI]) {
					enemySprites[realI].destroy();
					enemySprites[realI+1].destroy();
				}
				enemySprites[realI] = null;
				enemySprites[realI+1] = null;
			}
		}
	}
}

var clamp = function(num, min, max) {
    return num < min ? min : (num > max ? max : num);
};

var lastCursorUse = 0;

function useCursorH(i) {
	if(magicAnton > timer.now()) {
		return;
	}

	if(firstFrame) {
		firstFrame = false;
		return;
	}

	if(waitForKeyUp) {
		if(cursors.left.isUp && cursors.right.isUp) {
			waitForKeyUp = false;
		}
		return;
	}


	if((timer.now() - lastCursorUse) > 200) {
		if (cursors.left.isDown) {
			i--;
			lastCursorUse = timer.now();
		} else if (cursors.right.isDown) {
			i++;
			lastCursorUse = timer.now();
		}

		if(i >= player.abilities.length) {
			i = 0;
		} else if(i < 0) {
			i = player.abilities.length-1;
		}

		cursorH.x = abilitiesStartX + i * abiltiesDeltaX;
	
		if(zKey.isDown) {
			cursorH.visible = false;
			lastCursorUse = timer.now();
		}
	}
}



function basicAtk() {
	//console.log('atk');
	cursorV.visible = true;

	var enemies = battles[player.curBattle].enemies;

	var i = (cursorV.y - fighterStartY) / fighterDeltaY;

	if(!enemies[i]) {
		for(i = 0; i < enemies.length; i++) {
			if(enemies[i]) {
				break;
			}
		}
	}

	if((timer.now() - lastCursorUse) > 200) {
		if (cursors.up.isDown) {
			i = findNextEnemy(enemies, i, -1);
			lastCursorUse = timer.now();
		} else if (cursors.down.isDown) {
			i = findNextEnemy(enemies, i, 1);
			lastCursorUse = timer.now();
		}
	
		if(zKey.isDown) {
			ws.send(JSON.stringify({
				atk: 'basic',
				battle: player.curBattle,
				user: player.gameID,
				target: i,
				dmg: player.basicDmg
			}));

			cursorH.visible = true;
			cursorV.visible = false;
			lastCursorUse = timer.now();
		}

		if(xKey.isDown) {
			cursorH.visible = true;
			cursorV.visible = false;
			lastCursorUse = timer.now();
		}
	}

	cursorV.y = fighterStartY + i * fighterDeltaY;
}

function findNextEnemy(enemies, start, dir) {
	var realI = start + dir;

	for(var i = 0; i < enemies.length; i++) {
		if(realI >= enemies.length) {
			realI = 0;
		} else if(realI < 0) {
			realI = enemies.length-1;
		}

		if(enemies[realI]) {
			break;
		}

		realI += dir;
	}

	return realI;
}

var fleeSent = false;

function flee() {
	if(!fleeSent) {
		ws.send(JSON.stringify({
			atk: 'flee'
		}));

		fleeSent = true;
	}
}

})();