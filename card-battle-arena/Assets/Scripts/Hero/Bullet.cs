using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 8f;

	private HealthBar target;
	private float atkDmg;

	public void setUp(HealthBar hb, float atkDmg) {
		this.target = hb;
		this.atkDmg = atkDmg;
	}
	
	// Update is called once per frame
	void Update() {
		if (target == null) {
			Destroy(gameObject);
			return;
		}

		if (transform.position == target.transform.position) {
			target.applyDamage (atkDmg);
			Destroy (gameObject);
		} else {
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, step);
		}
	}
}
