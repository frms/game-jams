using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NearSensor : MonoBehaviour {

	public HashSet<Transform> targets = new HashSet<Transform>();

	private int selectionBoxLayer;
	
	// Use this for initialization
	void Start () {
		selectionBoxLayer = LayerMask.NameToLayer ("SelectionBox");
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer != selectionBoxLayer) {
			targets.Add (other.transform);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.layer != selectionBoxLayer) {
			targets.Remove (other.transform);
		}
	}
}
