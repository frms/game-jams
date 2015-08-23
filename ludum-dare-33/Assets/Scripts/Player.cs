using UnityEngine;
using System.Collections;

public class Player : Mover {

	public float atkRate = 0.5F;
	public float atkDmg = 5f;
	
	public Transform bullet;

	private HealthBar enemyHealth;
	private float nextFire = 0.0F;

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			target = getTarget();

			Vector3 endPos;

			if(target == null) {
				endPos = getMousePosition ();

				findPath(endPos);
			}

			if(target != null && target.tag == "Enemy") {
				enemyHealth = target.GetComponent<HealthBar>();
			} else {
				enemyHealth = null;
			}
		}

		moveUnit ();

		if (target != null && diagonalDist(reservedPos, target.reservedPos) <= (distToTarget + 0.5f)) {
			//Look at the target and stop moving
			steeringUtils.lookAtDirection (target.transform.position - transform.position);
			
			if (enemyHealth != null && Time.time > nextFire) {
				nextFire = Time.time + atkRate;
				Transform clone = Instantiate (bullet, transform.position, Quaternion.identity) as Transform;
				clone.GetComponent<Bullet> ().setUp (enemyHealth, atkDmg);
			}
		}
	}

	private Mover getTarget() {
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(r, out hit, Mathf.Infinity, GameManager.enemyMask)) {
			return hit.transform.GetComponent<Mover>();
		}

		return null;
	}

	private Vector3 getMousePosition () {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -1 * Camera.main.transform.position.z;

		return Camera.main.ScreenToWorldPoint (mousePos);
	}


	private static float D = 1;
	private static float D2 = Mathf.Sqrt (2) * D;
	
	public float diagonalDist(int[] node, int[] goal) {
		int dx = Mathf.Abs (node [0] - goal [0]);
		int dy = Mathf.Abs (node [1] - goal [1]);
		return D * (dx + dy) + (D2 - 2 * D) * Mathf.Min (dx, dy);
	}
}
