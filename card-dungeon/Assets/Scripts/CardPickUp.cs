using UnityEngine;
using System.Collections;

public class CardPickUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log (other.tag);
		
		if (other.name == "Player") {
			//other.gameObject.SendMessage("gainElement"+elementType);
			print ("Picked up card");
			Destroy(gameObject);
		}
	}
}
