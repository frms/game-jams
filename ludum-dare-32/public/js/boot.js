var boot = function(game){};

var cursors;
var aKey;
var zKey;
var xKey;

boot.prototype = {
	preload: function(){
		//this.game.load.image("loading","assets/loading.png"); 
	},
	create: function(){
		cursors = game.input.keyboard.createCursorKeys();
		aKey = game.input.keyboard.addKey(Phaser.Keyboard.A);
		zKey = game.input.keyboard.addKey(Phaser.Keyboard.Z);
		xKey = game.input.keyboard.addKey(Phaser.Keyboard.X);

		game.stage.disableVisibilityChange = true;

		game.stage.backgroundColor = '#83b92d';
		
		game.scale.scaleMode = Phaser.ScaleManager.SHOW_ALL;
		
		game.state.start("overworld");
	}
}