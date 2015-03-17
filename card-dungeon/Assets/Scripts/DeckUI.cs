using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckUI : MonoBehaviour {
	public Image image;
	public Text text;

	private RectTransform rt;

	private Dictionary<Card, Text> map;

	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform> ();
		map = new Dictionary<Card, Text> ();
	}

	public void addCard(Card c, int num) {
		Image childImg = makeChild(image) as Image;
		childImg.color = c.color;

		Text childText = makeChild (text) as Text;
		childText.text = num.ToString ();

		map [c] = childText;
	}

	public void setCount(Card c, int num) {
		map [c].text = num.ToString ();
	}

	private MaskableGraphic makeChild(MaskableGraphic elem) {
		MaskableGraphic child = Instantiate (elem) as MaskableGraphic;
		child.rectTransform.SetParent (rt, false);
		child.gameObject.SetActive (true);
		return child;
	}
}
