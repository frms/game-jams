using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public float speed = 5f;

	private Deck deck;

	private Hand handUI;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		deck = GameObject.Find ("DeckUI").GetComponent<Deck> ();
		deck.initializeDeck ();

		handUI = GameObject.Find ("HandUI").GetComponent<Hand> ();

		rb = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		updateMoveCharacter ();

		if (Input.GetButtonDown ("Fire1")) {
			print ("should be A");
		} else if (Input.GetButtonDown ("Fire2")) {
			print ("should be B");
		} else if (Input.GetButtonDown ("Fire3")) {
			print ("should be X");
		} else if (Input.GetButtonDown ("Jump")) {
			print ("should be Y");
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

	public void addCard(Card c) {
		if (handUI.isFull()) {
			deck.addCard (c);
		} else {
			handUI.addCard(c);
		}
	}
}
