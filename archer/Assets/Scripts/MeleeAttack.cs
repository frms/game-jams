using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("On Trigger " + other.gameObject.name);
	}
}
