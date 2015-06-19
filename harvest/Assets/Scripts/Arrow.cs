using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 6);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Capsule") {
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}
}
