"use strict";

let output
let zones;
let playerUI, enemyUI;
let player, enemy;

function display(str) {
	output.html(output.html() + str + '<br>');
	output[0].scrollTop = output[0].scrollHeight;
}

function nextBattle() {
	zones.update(player.level);
	enemy = zones.spawnCreature(enemyUI);

	display('-------------------------------');
}

function newGame() {
	zones = new Zones('#zones');
	player = new Creature(0, 5, playerUI);
	nextBattle();
}

function playerTurn(action) {
	let dmg = player.attack(enemy);
	display(`Player ${action} for ${dmg}`);
}

function enemyTurn() {
	let str = (Math.random() < 0.5) ? 'Enemy Punch' : 'Enemy Kick';
	let dmg = enemy.attack(player);
	display(str + ` for ${dmg}`)
}

function moveClick(e){
	playerTurn(this.innerText);
	enemyTurn();

	if(player.hp <= 0) {
		display('Player lost!');
		newGame();
	} else if(enemy.hp <= 0) {
		display('Player wins!');
		player.exp += enemy.defeatExp;
		nextBattle();
	}
}

$(document).ready(function() {
	output = $('#output');
	playerUI = new BattleUI('.player');
	enemyUI = new BattleUI('.enemy');
	newGame();

	$('.move').click(moveClick);
});