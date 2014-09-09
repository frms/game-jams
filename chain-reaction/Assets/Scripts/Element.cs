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
}
