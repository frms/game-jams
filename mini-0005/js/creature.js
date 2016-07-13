"use strict";

class HealthBar {
	constructor(selector) {
		let elem = $(selector);
		this.barElem = elem.find('.health-bar');
		this.textElem = elem.find('.health-text');
	}

	update(hp, maxHp) {
		this.textElem.text(hp);
		var percent = Math.max(100*hp/maxHp, 0);
		this.barElem.css('width', `${percent}%`);
	}
}

class Creature {
	constructor(healthBar, hp, dmg) {
		this.healthBar = healthBar;
		this.maxHp = hp;
		this.hp = hp;
		this.dmg = dmg;
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