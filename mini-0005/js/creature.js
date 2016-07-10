"use strict";

class Creature {
	constructor(hp, dmg) {
		this.hp = hp;
		this.dmg = dmg;
	}

	attack(target) {
		let dmg = (this.dmg * 0.95) + (this.dmg * Math.random() * 0.1);
		dmg = Math.round(dmg);
		target.hp -= dmg;
		return dmg;
	}

	toString() {
		return `${this.constructor.name} ` + JSON.stringify(this);
	}
}