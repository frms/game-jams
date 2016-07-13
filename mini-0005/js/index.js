"use strict";

let $output;
let playerHB, enemyHB;
let player, enemy;

function display(str) {
	$output.html($output.html() + str + '<br>');
	$output[0].scrollTop = $output[0].scrollHeight;
}

function nextBattle() {
	enemy = new Creature(enemyHB, 80, 15);

	display('-------------------------------');
}

function newGame() {
	player = new Creature(playerHB, 100, 20);
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
		nextBattle();
	}
}

$(document).ready(function() {
	$output = $('#output');
	playerHB = new HealthBar('.player.health');
	enemyHB = new HealthBar('.enemy.health');
	newGame();

	$('.move').click(moveClick);
});