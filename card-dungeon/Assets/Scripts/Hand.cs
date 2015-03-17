using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Hand : MonoBehaviour {
	public Image image;

	private RectTransform rt;

	public int handSize = 4;
	private List<Image> imgs;

	private List<Card> hand;

	// Use this for initialization
	void Awake () {
		rt = GetComponent<RectTransform> ();
		initializeCards ();
		hand = new List<Card> ();
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
		return hand.Count >= handSize; 
	}

	public void addCard(Card c) {
		Image img = imgs [hand.Count];
		img.color = c.color;
		img.enabled = true;

		hand.Add (c);
	}

	public Card removeCard(int handIndex) {
		Card card = null;

		if (handIndex >= 0 && handIndex < hand.Count) {
			card = hand[handIndex];
			hand.RemoveAt (handIndex);

			updateUI ();
		}

		return card;
	}

	private void updateUI() {
		for(int i = 0; i < handSize; i++) {
			if(i < hand.Count) {
				imgs[i].color = hand[i].color;
				imgs[i].enabled = true;
			} else {
				imgs[i].enabled = false;
			}
		}
	}
}
