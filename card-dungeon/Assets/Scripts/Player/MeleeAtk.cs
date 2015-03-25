using UnityEngine;
using System.Collections;

public class MeleeAtk : MonoBehaviour {
	public float dmg = 50f;

	private Collider2D meleeCircle;

	// Use this for initialization
	void Start () {
		meleeCircle = GetComponent<Collider2D> ();
		meleeCircle.enabled = false;
	}

	void FixedUpdate() {
		if (meleeCircle.enabled) {
			meleeCircle.enabled = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		IHealth health = other.GetComponent<IHealth> ();
		health.takeDamage (dmg);
	}

	public void attack() {
		meleeCircle.enabled = true;
	}
}
