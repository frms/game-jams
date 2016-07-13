"use strict";

class HealthBar {
	constructor(selector) {
		let elem = $(selector);
		this.barElem = elem.find('.health-bar');
		this.textElem = elem.find('.health-text');
	}

	update(hp, maxHp) {
		this.textElem.text(hp);
		let percent = Math.max(100*hp/maxHp, 0);
		this.barElem.css('width', `${percent}%`);
	}
}

class Creature {
	constructor(healthBar, num) {
		this.healthBar = healthBar;

		let stats = Creature.list[num];
		this.maxHp = stats.hp;
		this.hp = stats.hp;
		this.dmg = stats.dmg;
	}

	get hp() {
		return this.curHp;
	}

	set hp(val) {
		this.curHp = val;

		if(this.healthBar) {
			this.healthBar.update(this.curHp, this.maxHp)
		}
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
		hp: 80,
		dmg: 15
	},
	{
		hp: 30,
		dmg: 40
	},
	{
		hp: 55,
		dmg: 10
	}
];