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
	void FixedUpdate () {
		updateMoveCharacter ();

		// A Button
		if (Input.GetButtonDown ("Fire1")) {
			//useCardAndDraw(0);
			test();
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

	public void test() {
		MapData map = new MapData (3, 3);
		
		for (int j = 0; j < map.height; j++) {
			for (int i = 0; i < map.width; i++) {
				map[i,j] = 1;
			}
		}
		
		map [2, 0] = 0;
		map [0, 2] = 0;
		
		List<int[]> l;
		AStar a = new AStar();

		l = a.findPathAStar (map, new [] { 0, 0 }, new [] { 2, 2 });
		print(l);

		l = a.findPathAStar (map, new [] { 0, 0 }, new [] { 2, 0 });
		print(l);
	}
	
	private static void print(List<int[]> l) {
		if (l == null) {
			Debug.Log("No path found");
		} else {
			for (int i = 0; i < l.Count; i++) {
				Debug.Log ("[" + l [i] [0] + ", " + l [i] [1] + "]");
			}
		}
		Debug.Log ("-----------------------------------------------");
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
