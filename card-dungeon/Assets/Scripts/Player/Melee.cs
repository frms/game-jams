using UnityEngine;
using System.Collections;

public class Melee : MonoBehaviour {
	public float dmg = 50f;

	private Collider2D collider2d;
	private MeshRenderer meshRender;

	// Use this for initialization
	void Start () {
		collider2d = GetComponent<Collider2D> ();
		collider2d.enabled = false;

		meshRender = GetComponent<MeshRenderer> ();
		meshRender.enabled = false;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		IHealth health = other.GetComponent<IHealth> ();
		health.takeDamage (dmg);
	}

	public void use() {
		// If we are not already attacking then attack
		if (!collider2d.enabled) {
			collider2d.enabled = true;
			meshRender.enabled = true;

			Invoke ("stopAttack", 0.25f);
		}
	}

	public void stopAttack() {
		collider2d.enabled = false;
		meshRender.enabled = false;
	}
}
