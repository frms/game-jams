var fs = require('fs');

var map = JSON.parse(fs.readFileSync('./public/level1.json', 'utf8'));

for(var i = 0; i < map.layers.length; i++) {
	map.layers[i].getTile = getTile;
	map.layers[i].getObjects = getObjects;
}

function getTile(x, y) {
	return this.data[y*this.width + x];
}

function getObjects(type) {
	var result = [];
	for(var i = 0; i < this.objects.length; i++) {
		var element = this.objects[i];
		if(element.properties.type === type) {
			//Phaser uses top left, Tiled bottom left so we have to adjust the y position
			//also keep in mind that the cup images are a bit smaller than the tile which is 16x16
			//so they might not be placed in the exact pixel position as in Tiled
			element.y -= map.tileheight;
			result.push(element);
		}
	}
	return result;
}


map.getLayer = function getLayer(name) {
	for(var i = 0; i < this.layers.length; i++) {
		if(this.layers[i].name === name) {
			return this.layers[i];
		}
	}

	return null;
};

module.exports = map;