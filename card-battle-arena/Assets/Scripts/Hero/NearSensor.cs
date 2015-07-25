using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NearSensor : MonoBehaviour {

	public HashSet<Transform> targets = new HashSet<Transform>();

	void OnTriggerEnter2D(Collider2D other) {
		if (isTargetable(other)) {
			targets.Add (other.transform);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (isTargetable(other)) {
			targets.Remove (other.transform);
		}
	}

	private bool isTargetable(Collider2D coll) {
		return coll.gameObject.layer == GameManager.heroLayer || coll.gameObject.layer == GameManager.teamMember;
	}
}
