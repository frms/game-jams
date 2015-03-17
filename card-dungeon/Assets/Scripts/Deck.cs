using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Deck : MonoBehaviour {
	class CardCount {
		public Text text;
		private int c;
		public int count {
			get {
				return c;
			}
			set {
				text.text = value.ToString();
				c = value;
			}
		}

		public CardCount(Text t, int c) {
			text = t;
			count = c;
		}
	}

	public Image image;
	public Text text;

	private RectTransform rt;

	private Dictionary<Card, CardCount> deck = new Dictionary<Card, CardCount> ();
	private int deckSize = 0;

	// Use this for initialization
	void Awake () {
		rt = GetComponent<RectTransform> ();
	}

	public void initializeDeck() {
		List<Card> cards = GameObject.Find ("Map").GetComponent<TileMap> ().cards;

		for (int i = 0; i < cards.Count; i++) {
			initializeCard(cards[i], 0);
		}
		
		deckSize = 0;
	}

	public void initializeCard(Card c, int num) {
		Image childImg = makeChild(image) as Image;
		childImg.color = c.color;

		Text childText = makeChild (text) as Text;
		childText.text = num.ToString ();

		deck [c] = new CardCount (childText, num);
	}

	private MaskableGraphic makeChild(MaskableGraphic elem) {
		MaskableGraphic child = Instantiate (elem) as MaskableGraphic;
		child.rectTransform.SetParent (rt, false);
		child.gameObject.SetActive (true);
		return child;
	}

	public void addCard(Card c) {
		deck [c].count++;
		deckSize++;
	}

	public Card drawCard() {
		Card ret = null;

		if (deckSize > 0) {
			int r = Random.Range (0, deckSize);
			int sum = 0;

			foreach(Card c in deck.Keys) {
				sum += deck[c].count;

				if(r < sum) {
					ret = c;
					break;
				}
			}

			deck [ret].count--;
			deckSize--;
		}

		return ret;
	}
}
