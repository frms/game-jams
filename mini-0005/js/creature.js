"use strict";

class BattleUI {
	constructor(selector) {
		let elem = $(selector);
		this.name = elem.find('.name');
		this.level = elem.find('.level');
		this.healthBar = elem.find('.health-bar');
		this.healthText = elem.find('.health-text');
		this.expBar = elem.find('.exp-bar');
		this.expText = elem.find('.exp-text');
	}

	setUp(creature) {
		this.name.text(creature.name);
		this.update(creature)
	}

	update(creature) {
		this.level.text(`Lvl ${creature.level}`);

		this.healthText.text(creature.hp);
		let percent = Math.max(100*creature.hp/creature.maxHp, 0);
		this.healthBar.css('width', `${percent}%`);

		this.expText.text(`${creature.levelExp}/${creature.levelMaxExp}`);
		this.expBar.css('width', `${100*creature.levelExp/creature.levelMaxExp}%`);
	}
}

class Creature {

	constructor(num, lvl, battleUI) {
		this.base = Creature.list[num];
		this.name = this.base.name;
		this.exp = Math.pow(lvl, 3);

		this.battleUI = battleUI;
		this.battleUI.setUp(this);
	}

	updateUI() {
		if(this.battleUI) {
			this.battleUI.update(this)
		}
	}

	get hp() {
		return this.curHp;
	}

	set hp(val) {
		this.curHp = Math.round(val);
		this.updateUI();
	}

	get exp() {
		return this.curExp;
	}

	get level() {
		return Math.trunc(Math.cbrt(this.exp));
	}

	set exp(val) {
		let startLevel = this.level;
		this.curExp = val;

		if(startLevel != this.level) {
			let stats = this.base;
			this.maxHp = stats.hp * this.level;
			this.dmg = stats.dmg * this.level;

			this.hp = this.maxHp;
		}
		
		this.updateUI();
	}

	get levelExp() {
		return Math.trunc(this.exp - Math.pow(this.level, 3));
	}

	get levelMaxExp() {
		return Math.trunc(Math.pow(this.level + 1, 3) - Math.pow(this.level, 3));
	}

	get defeatExp() {
		let stats = this.base;
		return ((Math.sqrt(stats.hp) + Math.sqrt(stats.dmg)) * this.level) / 6;
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
		"name": "Angela",
		"hp": "1715.85083",
		"dmg": "666.5039063"
	},
	{
		"name": "Betty",
		"hp": "1274.108887",
		"dmg": "585.9375"
	},
	{
		"name": "Charlotte",
		"hp": "1705.932617",
		"dmg": "585.9375"
	},
	{
		"name": "Daisy",
		"hp": "1025.390625",
		"dmg": "527.34375"
	},
	{
		"name": "Evie",
		"hp": "843.8110352",
		"dmg": "1312.5"
	},
	{
		"name": "Freya",
		"hp": "769.0429688",
		"dmg": "153.8085938"
	},
	{
		"name": "Gracie",
		"hp": "923.9196777",
		"dmg": "205.078125"
	},
	{
		"name": "Holly",
		"hp": "1493.835449",
		"dmg": "0"
	},
	{
		"name": "Isla",
		"hp": "1319.885254",
		"dmg": "0"
	},
	{
		"name": "Jessica",
		"hp": "1243.591309",
		"dmg": "585.9375"
	},
	{
		"name": "Kaiden",
		"hp": "794.6777344",
		"dmg": "791.015625"
	},
	{
		"name": "Logan",
		"hp": "926.9714355",
		"dmg": "527.34375"
	},
	{
		"name": "Mia",
		"hp": "1812.744141",
		"dmg": "527.34375"
	},
	{
		"name": "Noah",
		"hp": "1025.390625",
		"dmg": "527.34375"
	},
	{
		"name": "Olivia",
		"hp": "1367.950439",
		"dmg": "546.875"
	},
	{
		"name": "Patrick",
		"hp": "2563.476563",
		"dmg": "1171.875"
	},
	{
		"name": "Queenie",
		"hp": "1094.360352",
		"dmg": "703.125"
	}
];