using UnityEngine;
using System.Collections;

public class Element : MonoBehaviour {

	public string elementType;
	
	
	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log (other.tag);
		
		if (other.tag == "Player") {
			other.gameObject.SendMessage("gainElement"+elementType);
			Destroy(gameObject);
		}
	}

	private float endOfLife = -1;
	private float lifeDuration = 3;

	void Update() {
		if(endOfLife == -1) {
			endOfLife = Time.time + lifeDuration;
		}

		if(endOfLife < Time.time) {
			Destroy(gameObject);
		}
	}
}
