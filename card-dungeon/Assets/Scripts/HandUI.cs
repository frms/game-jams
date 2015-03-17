using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HandUI : MonoBehaviour {
	public Image image;
	
	private RectTransform rt;
	
	private List<Card> hand;
	
	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform> ();
		hand = new List<Card> ();
	}
	
	public void addCard(Card c) {
		Image childImg = makeChild(image) as Image;
		childImg.color = c.color;
		hand.Add (c);
	}
	
	private MaskableGraphic makeChild(MaskableGraphic elem) {
		MaskableGraphic child = Instantiate (elem) as MaskableGraphic;
		child.rectTransform.SetParent (rt, false);
		child.gameObject.SetActive (true);
		return child;
	}
}
