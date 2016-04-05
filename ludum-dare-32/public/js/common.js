var timer = {};
timer.setTime = function setTime(t) {
	this.lastTime = t;
	this.lastSystemTime = performance.now();
};
timer.now = function now() {
	var dt = performance.now() - this.lastSystemTime;
	return this.lastTime + dt;
};

var MAX_IN_BATTLE = 4;