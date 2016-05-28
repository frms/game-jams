/**
 * Wall 0 is no wall
 * Wall 1 is no walking but no additional image
 * Wall 2 is rock 
 */

var sections = [
	[
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":1,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":2},{"tile":0,"wall":2},{"tile":0,"wall":2},{"tile":0,"wall":2},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":2},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":2},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":2},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":2},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":2},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":2},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":2,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":2},{"tile":0,"wall":2},{"tile":0,"wall":2},{"tile":0,"wall":2},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}]
	],
	[
		[{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0}],
		[{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":1,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}]
	],
	[
		[{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":1,"wall":0},{"tile":0,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":1,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":2,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":1,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":1,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":2,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":1,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":2,"wall":0},{"tile":0,"wall":0},{"tile":1,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":3,"wall":0},{"tile":3,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":1,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":2,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}]
	],
	[
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0},{"tile":1,"wall":0},{"tile":1,"wall":0},{"tile":2,"wall":0},{"tile":3,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":2,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":3,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":1,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}],
		[{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":3,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":2,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":1,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0},{"tile":0,"wall":0}]
	]
];

var sectionWalls = [];

generateSectionWallData();

function generateSectionWallData() {
	for(var s in sections) {
		var walls = [];
		for(var r in sections[s]) {
			for(var c in sections[s][r]) {
				if(sections[s][r][c].wall > 0) {
					walls.push({row:r, col:c});
				}
			}
		}
		sectionWalls.push(walls);
	}
	
	//console.log(sectionWalls);
}