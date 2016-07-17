"use strict";

class Zones {
	constructor(selector) {
		this.elem = $(selector);
		this.elem.empty();
	}

	get count() {
		return this.elem.children().length;
	}

	get current() {
		return this.currentZone;
	}

	set current(i) {
		this.elem.children().removeClass('current');
		$(this.elem.children()[i]).addClass('current');
		this.currentZone = i; 
	}

	createZone(i) {
		let zone = $(`<div class="zone button">${i}</div>`);
		
		let that = this;
		zone.click(function() {
			that.current = i;
		});
		
		return zone;
	}

	update(playerLevel) {
		for(let i = this.count; i < Zones.list.length; i++) {
			if(playerLevel >= Zones.list[i].levelRange[0] + 3) {
				this.elem.append(this.createZone(i));

				if (typeof this.current === 'undefined' || this.current === i - 1) {
					this.current = i;
				}
			} else {
				break;
			}
		}
	}

	spawnCreature(enemyUI) {
		let zone = Zones.list[this.current];
		let level = Math.trunc(Math.random() * (zone.levelRange[1] - zone.levelRange[0])) + zone.levelRange[0];

		let num;
		let sum = 0;		
		let rand = Math.random();
		
		for(let i = 0; i < zone.spawns.length; i++) {
			sum += zone.spawns[i].chance;
			
			if(rand <= sum) {
				num = zone.spawns[i].num;
				break;
			}
		}

		return new Creature(num, level, enemyUI);
	}
}


Zones.list = [
	{
		levelRange: [2,5],
		spawns: [
			{
				num: 1,
				chance: 0.4
			},
			{
				num: 2,
				chance: 0.4
			},
			{
				num: 3,
				chance: 0.2
			}
		]
	},
	{
		levelRange: [3,6],
		spawns: [
			{
				num: 3,
				chance: 0.5
			},
			{
				num: 4,
				chance: 0.25
			},
			{
				num: 5,
				chance: 0.25
			}
		]
	},
	{
		levelRange: [5,8],
		spawns: [
			{
				num: 1,
				chance: 0.45
			},
			{
				num: 6,
				chance: 0.35
			},
			{
				num: 7,
				chance: 0.2
			}
		]
	},
	{
		levelRange: [6,11],
		spawns: [
			{
				num: 8,
				chance: 0.1
			},
			{
				num: 9,
				chance: 0.7
			},
			{
				num: 10,
				chance: 0.1
			},
			{
				num: 11,
				chance: 0.1
			}
		]
	},
	{
		levelRange: [7,11],
		spawns: [
			{
				num: 8,
				chance: 0.15
			},
			{
				num: 9,
				chance: 0.4
			},
			{
				num: 10,
				chance: 0.3
			},
			{
				num: 11,
				chance: 0.15
			}
		]
	},
	{
		levelRange: [9,12],
		spawns: [
			{
				num: 8,
				chance: 0.2
			},
			{
				num: 9,
				chance: 0.3
			},
			{
				num: 10,
				chance: 0.2
			},
			{
				num: 11,
				chance: 0.3
			}
		]
	}
];