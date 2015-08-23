using UnityEngine;
using System.Collections;

public class Player : Mover {

	public float critChance = 0.3f;
	public float[] critMult = new float[]{1.5f, 2.2f};

	public GameObject gameOverPanel;

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

		playerTryToAttack();

//		float v = GetComponent<Rigidbody>().velocity.magnitude;
//		if(v > 0) {
//			Debug.Log(name + " " + v);
//		}
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

	public void playerTryToAttack() {
		if (enemyHealth != null && diagonalDist(reservedPos, target.reservedPos) <= distToTarget) {
			//Look at the target and stop moving
			steeringUtils.lookAtDirection (target.transform.position - transform.position);
			
			if (Time.time > nextFire) {
				nextFire = Time.time + atkRate;
				Transform clone = Instantiate (bullet, transform.position + Bullet.aboveGround, Quaternion.identity) as Transform;

				float dmg = atkDmg;

				if(Random.value < critChance) {
					dmg *= Random.Range(critMult[0], critMult[1]);
				}

				clone.GetComponent<Bullet> ().setUp (enemyHealth, dmg);
			}
		}
	}

	void OnDestroy() {
		if(gameOverPanel != null) {
			gameOverPanel.SetActive(true);
		}
	}
}
