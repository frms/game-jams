using UnityEngine;
using System.Collections.Generic;

public abstract class Ability : MonoBehaviour {
	public int playerIndex;
	internal CoolDown coolDown;
	internal int memberMask;

	public float sleepTime = -1;
	private float sleepStartTime;


	public void Awake () {
		coolDown = GetComponent<CoolDown>();
		memberMask = (1 << LayerMask.NameToLayer("Member"));
	}

	public virtual void setUp(Dictionary<string, object> settings) {
		foreach(KeyValuePair<string, object> item in settings)
		{
			if(item.Key.Equals("sleepTime")) {
				this.sleep((float) item.Value);
			} else if(item.Key.Equals("coolDownTime")) {
				coolDown.time = (float) item.Value;
			} else if(item.Key.Equals("health")) {
				Health h = GetComponent<Health>();
				h.barMax = (float) item.Value;
				h.barProgress = h.barMax;
			}
		}
	}

	public virtual void Update () {
		// If we have a sleep time and we've already slept long enough clear the sleep time
		if(sleepTime >= 0 && isDoneSleeping()) {
			clearSleep();
		}
	}

	public bool isDoneSleeping() {
		return (Time.time - sleepStartTime) >= sleepTime;
	}

	public bool isReady() {
		return coolDown.isReady() && isDoneSleeping();
	}

	public void resetCoolDown() {
		coolDown.resetCoolDown();
	}

	public void sleep(float time) {
		sleepTime = time;
		sleepStartTime = Time.time;
		coolDown.paused = true;
	}

	private void clearSleep() {
		sleepTime = -1;
		coolDown.paused = false;
	}

	public abstract bool handleUserInput();
}

public abstract class DmgAbility : Ability {
	public float dmg = 30;

	public override void setUp(Dictionary<string, object> settings) {
		base.setUp (settings);

		foreach(KeyValuePair<string, object> item in settings)
		{
			if(item.Key.Equals("dmg")) {
				dmg = (float) item.Value;
			}
		}
	}
}

public interface SingleTargetUse {
	void use(GameObject go);
}

public interface PointUse {
	void use(Vector3 point);
}