using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public float speed = 5f;

	public Dictionary<Card, int> deck;
	private int deckSize = 0;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		initializeDeck ();

		rb = GetComponent<Rigidbody2D> ();
	}

	private void initializeDeck() {
		deck = new Dictionary<Card, int> ();

		List<Card> cards = GameObject.Find ("Map").GetComponent<TileMap> ().cards;
		for (int i = 0; i < cards.Count; i++) {
			deck.Add(cards[i], 0);
		}

		deckSize = 0;
	}

	// Update is called once per frame
	void Update () {
		updateMoveCharacter ();
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
		deck [c] = deck [c] + 1;
		print (deck [c]);
	}
}
