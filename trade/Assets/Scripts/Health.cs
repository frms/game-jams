using UnityEngine;
using System.Collections;

public class Health : Bar {

	CoolDown coolDownBar;

	public override void Start() {
		base.Start();

		coolDownBar = gameObject.GetComponent<CoolDown>();
	}

	public void applyDamage(float damage) {
		barProgress -= damage;

		// Kill the game obj if it loses all its health
		if(barProgress <= 0) {
			Destroy(gameObject);
			Destroy(bar.gameObject);

			if(coolDownBar != null) {
				Destroy(coolDownBar.bar.gameObject);
			}
		}
	}
}
