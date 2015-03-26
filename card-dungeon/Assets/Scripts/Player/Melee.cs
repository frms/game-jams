using UnityEngine;
using System.Collections;

public class Melee : MonoBehaviour {
	public float dmg = 50f;

	private Collider2D collider;

	// Use this for initialization
	void Start () {
		collider = GetComponent<Collider2D> ();
		collider.enabled = false;
	}

	bool hasOneFramePassed = false;

	void FixedUpdate() {
		if (collider.enabled) {
			// Only disable the melee circle if there has already been 1 physics frame with it enabled.
			if (hasOneFramePassed) {
				collider.enabled = false;
			}
			// Else this is the that first frame of it enabled
			else {
				hasOneFramePassed = true;
			}
		} else {
			hasOneFramePassed = false;
		}
	}

	private int count = 0;
	void OnTriggerEnter2D(Collider2D other) {
		IHealth health = other.GetComponent<IHealth> ();
		health.takeDamage (dmg);
		count++;
		Debug.Log("Enemy " + count);
	}

	public void attack() {
		collider.enabled = true;
	}
}
