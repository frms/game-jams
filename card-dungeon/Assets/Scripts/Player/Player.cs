﻿
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour, IHealth {
	public float health = 100f;
	public float maxHealth = 100f;

	public float speed = 5f;

	private Deck deck;

	private Hand hand;

	private Slider healthUI;

	private Rigidbody2D rb;

	private Melee melee;
	private Laser laser;

	// Use this for initialization
	void Start () {
		deck = GameObject.Find ("DeckUI").GetComponent<Deck> ();
		deck.initializeDeck ();

		hand = GameObject.Find ("HandUI").GetComponent<Hand> ();

		healthUI = GameObject.Find ("PlayerHealth").GetComponent<Slider> ();

		rb = GetComponent<Rigidbody2D> ();

		melee = GetComponentInChildren<Melee> ();
		laser = GetComponentInChildren<Laser> ();
	}

	// Update is called once per frame
	void Update() {
		// A Button
		if (Input.GetButtonDown ("Fire1")) {
			useCardAndDraw(0);
			melee.use();
		}
		// B Button
		else if (Input.GetButtonDown ("Fire2")) {
			useCardAndDraw(1);
			laser.use();
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
	void FixedUpdate () {
		updateMoveCharacter ();
	}

	private void updateMoveCharacter() {
		Vector2 stickDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		rb.velocity = Vector2.zero;

		if(stickDirection.sqrMagnitude > 0.031) {
			
			// Rotate the character
			float moveAngle = Mathf.Atan2(stickDirection.y, stickDirection.x)*Mathf.Rad2Deg;
			if(moveAngle < 0) {
				moveAngle += 360;
			}

			transform.rotation = Quaternion.Euler(0, 0, moveAngle);

			if(Input.GetAxis ("Left Trigger") == 0 && Input.GetAxis ("Right Trigger") == 0) {
				rb.velocity = transform.right * speed;
			}	
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

	public void takeDamage(float dmg) {
		health -= dmg;
		healthUI.value = health / maxHealth;
	}
}
