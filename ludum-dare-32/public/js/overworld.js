var overworld = function(game){};

var overworldGroup;
var map;
var backgroundlayer;
var blockedLayer;
var player;

(function() {

overworld.prototype = { 
	preload: preload,
	create: create,
	update: update,
	render: render
};

function preload() {
	if(!overworldGroup) {
		game.load.tilemap('level1', 'level1.json', null, Phaser.Tilemap.TILED_JSON);
		game.load.image('tileSet', 'images/tileSet.png');
		game.load.spritesheet('dude', 'images/walking.png', 32, 50);
		game.load.image('playerBattle', 'images/rabbit.png');
		game.load.image('enemyBattle', 'images/enemyBattle.png');
		game.load.image('healthBar', 'images/healthBar.png');
		game.load.image('basicAtk', 'images/basicAtk.png');
		game.load.image('cursorH', 'images/cursorH.png');
		game.load.image('cursorV', 'images/cursorV.png');
		game.load.image('flee', 'images/flee.png');
		game.load.image('battleIcon', 'images/battleIcon.png');
		//game.load.audio('antonMusic', ['music.mp3']);
	}
}

function create() {
	if(!overworldGroup) {
		overworldGroup = game.add.group();

		map = game.add.tilemap('level1');

		//the first parameter is the tileset name as specified in Tiled, the second is the key to the asset
		map.addTilesetImage('tileSet', 'tileSet');

		//create layer
		backgroundlayer = map.createLayer('backgroundLayer');
		blockedLayer = map.createLayer('blockedLayer');

		overworldGroup.add(backgroundlayer);
		overworldGroup.add(blockedLayer);

		//var music = game.add.audio('antonMusic');

	    //music.play();
	} else {
		game.camera.follow(player);
	}

	overworldGroup.visible = true;

	aKey.onDown.removeAll();
	aKey.onDown.add(aPressed, this);

	//resizes the game world to match the layer dimensions
	backgroundlayer.resizeWorld();
}

function aPressed() {
	// var posStr = blockedLayer.getTileX(player.position.x) + ' ' + blockedLayer.getTileY(player.position.y);

	// ws.send(JSON.stringify({
	// 	joinBattle: posStr
	// }));
}

function update() {
	fpsStats.end();

	fpsStats.begin();

	if(!player) {
		setUpGame();
		return;
	}

	updateWorldState();

	// Process Input
	player.move(now, dt);
}

function setUpGame() {
	if(setUpData) {
		// The player and its settings
		player = new Player(setUpData.id, setUpData.move[0], setUpData.move[1]);

		game.camera.follow(player);

		for(var i = 0; i < setUpData.worldState.length; i++) {
			handleWorldMessage(setUpData.worldState[i]);
		}

		for(var i in setUpData.battles) {
			handleBattle(setUpData.battles[i]);
		}
	}
}

function render () {
	// game.debug.text('Click / Tap to go fullscreen', 270, 16);
	// game.debug.text('Click / Tap to go fullscreen', 0, 16);

	// game.debug.inputInfo(32, 32);
	// game.debug.pointer(game.input.activePointer);
}

})();