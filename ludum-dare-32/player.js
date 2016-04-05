Player = function Player () {};

Player.prototype.applyInput = function applyInput(input) {
	// if(input.moveDir) {
	// 	this.translateZ(input.moveDir * this.moveSpeed * input.dt);
	// }
	// if(input.turnDir) {
	// 	// Change the rotation this way and not with this.rotateY() because 
	// 	// the second way results in the server and client having slightly
	// 	// different positions
	// 	this.rotation.y += input.turnDir * this.turnSpeed * input.dt;
	// }

	// this.lastInputTime = input.time;
};

Player.prototype.getPlayerState = function getPlayerState() {	
	return {
		id: this.gameID,
		move: this.position
	};
}

Player.prototype.removeFromBattle = function removeFromBattle() {
	var result = null;

	if(this.curBattle) {
		var players = battles[this.curBattle].players;
		var index = players.indexOf(this.gameID);

		if(index !== -1) {
			result = battles[this.curBattle];

			players.splice(index, 1)[0];
			delete battles[this.curBattle].playersHealth[this.gameID];

			if(players.length === 0) {
				delete battles[this.curBattle];
				result.players = null;
			}

			this.curBattle = null;
		}
	}

	return result;
};

function getRandomInt(min, max) {
  return Math.floor(Math.random() * (max - min)) + min;
}

Battle = function Battle(battleIndex) {
	this.inBattle = battleIndex;
	this.players = [];
	this.enemies = [];
	this.playersHealth = {};

	var numEnemies = getRandomInt(1, 4);

	for(var i = 0; i < numEnemies; i++) {
		this.enemies[i] = new Enemy();
	}
}

Battle.prototype.getShortState = function getShortState() {
	return { 
		inBattle: this.inBattle,
		players: this.players
	};
};

Battle.prototype.addPlayer = function addPlayer(player) {
	this.players.push(player.gameID);
	player.curBattle = this.inBattle;
	this.playersHealth[player.gameID] = 100;
}

Battle.prototype.enemiesLeft = function enemiesLeft() {
	for(var i = 0; i < this.enemies.length; i++) {
		if(this.enemies[i]) {
			return true;
		}
	}

	return false;
}

var timeBetweenAtks = 3500;

Enemy = function Enemy() {
	this.health = 100;
	this.lastAtkTime = timer.now() - Math.floor(Math.random() * timeBetweenAtks/2);
}