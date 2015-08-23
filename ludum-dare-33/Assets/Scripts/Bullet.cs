using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public static Vector3 aboveGround = 0.3f * Vector3.back;

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

		if ((transform.position.x == target.transform.position.x) && (transform.position.y == target.transform.position.y)) {
			target.applyDamage (atkDmg);
			Destroy (gameObject);
		} else {
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position + aboveGround, step);
		}
	}
}
