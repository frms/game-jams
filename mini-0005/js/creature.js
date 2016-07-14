"use strict";

class BattleUI {
	constructor(selector) {
		let elem = $(selector);
		this.nameElem = elem.find('.name');
		this.levelElem = elem.find('.level');
		this.barElem = elem.find('.health-bar');
		this.textElem = elem.find('.health-text');
	}

	setUp(creature) {
		this.nameElem.text(creature.name);
		this.update(creature)
	}

	update(creature) {
		this.levelElem.text(`Lvl ${creature.level}`);
		this.textElem.text(creature.hp);
		let percent = Math.max(100*creature.hp/creature.maxHp, 0);
		this.barElem.css('width', `${percent}%`);
	}
}

class Creature {
	constructor(num, lvl, battleUI) {
		this.name = num;

		let stats = Creature.list[num];
		this.maxHp = stats.hp;
		this.hp = stats.hp;
		this.dmg = stats.dmg;

		this.exp = Math.pow(lvl, 3);

		this.battleUI = battleUI;
		this.battleUI.setUp(this);
	}

	get hp() {
		return this.curHp;
	}

	set hp(val) {
		this.curHp = Math.round(val);

		if(this.battleUI) {
			this.battleUI.update(this)
		}
	}

	get level() {
		return Math.trunc(Math.cbrt(this.exp));
	}

	attack(target) {
		let dmg = (this.dmg * 0.95) + (this.dmg * Math.random() * 0.1);
		dmg = Math.round(dmg);
		target.hp -= dmg;
		return dmg;
	}

	toString() {
		//return `${this.constructor.name} ` + JSON.stringify(this);
		return `${this.constructor.name} {hp:${this.hp}, dmg:${this.dmg}}`;
	}
}

Creature.list = [
	{
		"hp": "1715.85083",
		"dmg": "666.5039063"
	},
	{
		"hp": "1274.108887",
		"dmg": "585.9375"
	},
	{
		"hp": "1705.932617",
		"dmg": "585.9375"
	},
	{
		"hp": "1025.390625",
		"dmg": "527.34375"
	},
	{
		"hp": "843.8110352",
		"dmg": "1312.5"
	},
	{
		"hp": "769.0429688",
		"dmg": "153.8085938"
	},
	{
		"hp": "923.9196777",
		"dmg": "205.078125"
	},
	{
		"hp": "1493.835449",
		"dmg": "0"
	},
	{
		"hp": "1319.885254",
		"dmg": "0"
	},
	{
		"hp": "1243.591309",
		"dmg": "585.9375"
	},
	{
		"hp": "794.6777344",
		"dmg": "791.015625"
	},
	{
		"hp": "926.9714355",
		"dmg": "527.34375"
	},
	{
		"hp": "1812.744141",
		"dmg": "527.34375"
	},
	{
		"hp": "1025.390625",
		"dmg": "527.34375"
	},
	{
		"hp": "1367.950439",
		"dmg": "546.875"
	},
	{
		"hp": "2563.476563",
		"dmg": "1171.875"
	},
	{
		"hp": "1094.360352",
		"dmg": "703.125"
	}
];