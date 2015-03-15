using UnityEngine;
using System.Collections;

public class CoolDown : Bar {
	public float time = 2;
	internal float timeLeft;

	public bool paused = false;

	public override void Start() {
		base.Start();
		
		barProgress = 0;

		timeLeft = time;
	}

	public override void Update() {
		if(!paused) {
			float percentReady = Mathf.Min(time - timeLeft, time) / time;

			barProgress = percentReady * barMax;

			timeLeft -= Time.deltaTime;
		}

		base.Update();
	}

	public bool isReady() {
		return (time - timeLeft) >= time;
	}

	public void resetCoolDown() {
		timeLeft = time;
	}
}
