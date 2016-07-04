var $output;

$(document).ready(function() {
	$output = $('#output');

	$('#controls a').click(function(e){
		playerTurn(this.innerText);
		enemyTurn();
		e.preventDefault();
	})
});

function display(str) {
	$output.html($output.html() + str + '<br>');
	$output[0].scrollTop = $output[0].scrollHeight;
}

function playerTurn(action) {
	display('Player ' + action);
}

function enemyTurn() {
	if(Math.random() < 0.5) {
		display('Enemy Punch');
	} else {
		display('Enemy Kick');
	}
}