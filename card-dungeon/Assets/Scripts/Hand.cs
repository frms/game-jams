using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Hand : MonoBehaviour {
	public Image image;
	private List<Image> imgs;

	private RectTransform rt;
	
	private List<Card> hand;
	private int handSize;

	// Use this for initialization
	void Start () {
		imgs = new List<Image> ();
		rt = GetComponent<RectTransform> ();
		hand = new List<Card> ();
	}

	public void initialize(int handSize) {
		this.handSize = handSize;

		for(int i = 0; i < handSize; i++) {
			Image child = Instantiate (image) as Image;
			child.rectTransform.SetParent (rt, false);
			child.gameObject.SetActive(true);
			imgs.Add(image);
		}
	}

	public void addCard(Card c) {
//		Image childImg = makeChild(image) as Image;
//		childImg.color = c.color;
//		hand.Add (c);
	}
}
