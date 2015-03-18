using UnityEngine;
using System.Collections;

public class CardPickUp : MonoBehaviour {

	public Card card;
	
	public void setUp(Card c) {
		card = c;
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		sr.color = card.color;
	}
	
	void OnTriggerEnter(Collider other) {
		//Debug.Log (other.tag);
		
		if (other.name == "Player") {
			other.gameObject.GetComponent<Player>().addCard(card); 
			Destroy(gameObject);
		}
	}
}
