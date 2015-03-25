using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Hand : MonoBehaviour {
	public Image image;

	private RectTransform rt;

	public int handSize = 4;
	private List<Image> imgs;

	private Card[] hand;

	// Use this for initialization
	void Awake () {
		rt = GetComponent<RectTransform> ();
		initializeCards ();
		hand = new Card[handSize];
	}

	private void initializeCards() {
		imgs = new List<Image> ();

		for(int i = 0; i < handSize; i++) {
			Image child = Instantiate (image) as Image;
			child.rectTransform.SetParent (rt, false);
			child.gameObject.SetActive(true);
			imgs.Add(child);
		}
	}

	public bool isFull() {
		return firstOpenPosition() == -1; 
	}

	public int firstOpenPosition() {
		for (int i = 0; i < hand.Length; i++) {
			if(hand[i] == null) {
				return i;
			}
		}
		
		return -1;
	}

	public void addCard(Card c) {
		int i = firstOpenPosition ();
		hand [i] = c;

		Image img = imgs [i];
		img.color = c.color;
		img.enabled = true;
	}

	public Card removeCard(int handIndex) {
		Card card = hand[handIndex];

		hand [handIndex] = null;
		imgs [handIndex].enabled = false;

		return card;
	}
	
}
