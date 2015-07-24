using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NearSensor : MonoBehaviour {

	public HashSet<Transform> targets = new HashSet<Transform>();

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer != GameManager.selectionBoxLayer) {
			targets.Add (other.transform);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.layer != GameManager.selectionBoxLayer) {
			targets.Remove (other.transform);
		}
	}
}
