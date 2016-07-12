"use strict";

let $output;
let player, enemy;

$(document).ready(function() {
	$output = $('#output');
	newGame();

	$('.move').click(function(e){
		playerTurn(this.innerText);
		enemyTurn();
		display(`${player} ${enemy}`);

		if(player.hp <= 0) {
			display('Player lost!');
			newGame();
		} else if(enemy.hp <= 0) {
			display('Player wins!');
			nextBattle();
		}
	})
});

function newGame() {
	player = new Creature(100, 20);
	nextBattle();
}
function nextBattle() {
	enemy = new Creature(80, 15);

	display('-------------------------------');
	display('Battle begins!');
	display(`${player} ${enemy}`);
}

function display(str) {
	$output.html($output.html() + str + '<br>');
	$output[0].scrollTop = $output[0].scrollHeight;
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