using UnityEngine;
using System.Collections;

public class EnemyLaser : MonoBehaviour {
	public float dps = 10;

	private LineRenderer gunLine;
	private IHealth targetHealth;

	// Use this for initialization
	void Start () {
		gunLine = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gunLine.enabled) {
			targetHealth.takeDamage(dps*Time.deltaTime);
		}
	}

	private Collider2D target;

	public void fire(RaycastHit2D hit) {
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		gunLine.SetPosition (1, hit.point);

		if (target != hit.collider) {
			target = hit.collider;
			targetHealth = hit.collider.GetComponent<IHealth>();
		}
	}

	public void stop() {
		gunLine.enabled = false;
		target = null;
		targetHealth = null;
	}
}
