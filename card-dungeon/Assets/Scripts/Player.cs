using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public float speed = 5f;

	private Deck deck;

	private Hand hand;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		deck = GameObject.Find ("DeckUI").GetComponent<Deck> ();
		deck.initializeDeck ();

		hand = GameObject.Find ("HandUI").GetComponent<Hand> ();

		rb = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		updateMoveCharacter ();

		// A Button
		if (Input.GetButtonDown ("Fire1")) {
			useCardAndDraw(0);
		}
		// B Button
		else if (Input.GetButtonDown ("Fire2")) {
			useCardAndDraw(1);
		}
		// X Button
		else if (Input.GetButtonDown ("Fire3")) {
			useCardAndDraw(2);
		}
		// Y Button
		else if (Input.GetButtonDown ("Jump")) {
			useCardAndDraw(3);
		}
	}

	private void updateMoveCharacter() {
		Vector2 stickDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		
		if(stickDirection.sqrMagnitude > 0.031) {
			
			// Rotate the character
			float moveAngle = Mathf.Atan2(stickDirection.y, stickDirection.x)*Mathf.Rad2Deg;
			if(moveAngle < 0) {
				moveAngle += 360;
			}

			transform.rotation = Quaternion.Euler(0, 0, moveAngle);
			rb.velocity = transform.right * speed;
			
		} else {
			rb.velocity = Vector2.zero;
		}
	}

	private void useCardAndDraw(int handIndex) {
		Card c = hand.removeCard (handIndex);

		if (c != null) {
			deck.addCard(c);
			hand.addCard(deck.drawCard());
		}
	}

	public void addCard(Card c) {
		if (hand.isFull()) {
			deck.addCard (c);
		} else {
			hand.addCard(c);
		}
	}
}
