"use strict";

let output
let playerUI, enemyUI;
let player, enemy;

function display(str) {
	output.html(output.html() + str + '<br>');
	output[0].scrollTop = output[0].scrollHeight;
}

function nextBattle() {
	let num = Math.trunc(Math.random() * Creature.list.length);
	enemy = new Creature(num, 1, enemyUI);

	display('-------------------------------');
}

function newGame() {
	player = new Creature(0, 1, playerUI);
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