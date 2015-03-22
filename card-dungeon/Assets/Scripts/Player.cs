
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public float speed = 5f;

	private Deck deck;

	private Hand hand;

	private Rigidbody2D rb;

	public MapData map;

	// Use this for initialization
	void Start () {
		deck = GameObject.Find ("DeckUI").GetComponent<Deck> ();
		deck.initializeDeck ();

		hand = GameObject.Find ("HandUI").GetComponent<Hand> ();

		rb = GetComponent<Rigidbody2D> ();

		map = GameObject.Find ("Map").GetComponent<TileMap> ().map;
	}

	// Update is called once per frame
	void FixedUpdate () {
		updateMoveCharacter ();

		// A Button
		if (Input.GetButtonDown ("Fire1")) {
			//useCardAndDraw(0);
			setStart();
		}
		// B Button
		else if (Input.GetButtonDown ("Fire2")) {
			//useCardAndDraw(1);
			findPath();
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

	private int[] start;

	private void setStart() {
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		start = new int[2];
		start [0] = Mathf.FloorToInt (pos.x);
		start [1] = Mathf.FloorToInt (pos.y);
		print (start);
	}

	private void findPath() {
		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		sw.Start();

		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		int[] end = new int[2];
		end [0] = Mathf.FloorToInt (pos.x);
		end [1] = Mathf.FloorToInt (pos.y);
		print (end);

		AStar a = new AStar();
		
		List<int[]> l = a.findPathAStar (map, start, end);
		draw(l);

		sw.Stop();
		UnityEngine.Debug.Log (sw.ElapsedMilliseconds);
	}

	private static void draw(List<int[]> l) {
		if (l == null) {
			Debug.Log("No path found");
		} else {
			for (int i = 0; i < l.Count-1; i++) {
				Vector3 p1 = new Vector3(l[i][0] + 0.5f, l[i][1] + 0.5f, 0);
				Vector3 p2 = new Vector3(l[i+1][0] + 0.5f, l[i+1][1] + 0.5f, 0);

				Debug.DrawLine(p1, p2, Color.cyan, Mathf.Infinity, false);
			}
		}
		Debug.Log ("-----------------------------------------------");
	}

	private static void print(List<int[]> l) {
		if (l == null) {
			Debug.Log("No path found");
		} else {
			for (int i = 0; i < l.Count; i++) {
				print (l[i]);
			}
		}
		Debug.Log ("-----------------------------------------------");
	}

	private static void print(int[] arr) {
		Debug.Log ("[" + arr[0] + ", " + arr[1] + "]");
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
