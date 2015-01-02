using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Step : MonoBehaviour {
	public string title;

	public Sprite[] images;
	private int index;
	
	private SpriteRenderer[] spriteRenderers;

	private Text titleText;
	private Text labelText;

	// Use this for initialization
	void Awake () {
		titleText = GameObject.Find ("title").GetComponent<Text> ();
		labelText = GameObject.Find ("label").GetComponent<Text> ();
	}

	public void setUp(StepInspector si) {
		title = si.path;
		images = Resources.LoadAll<Sprite>(si.path);
		index = 0;

		spriteRenderers = new SpriteRenderer[si.sortingOrders.Length];

		GameObject stepObj = new GameObject (title);

		for(int i = 0; i < si.sortingOrders.Length; i++) {
			GameObject imgGameObj = new GameObject (title+"Child"+i);
			spriteRenderers[i] = imgGameObj.AddComponent<SpriteRenderer> ();
			spriteRenderers[i].sortingOrder = si.sortingOrders[i];

			imgGameObj.transform.parent = stepObj.transform;
		}
	}
	
	public void next() {
		index += spriteRenderers.Length;
		index %= images.Length;
		showCurrent();
	}
	
	public void prev() {
		index -= spriteRenderers.Length;
		index = (index < 0) ? images.Length + index : index;
		showCurrent ();
	}
	
	/**
	 * Shows the current prefab and returns the current prefab's name.
	 */
	public void showCurrent() {
		for(int i = 0; i < spriteRenderers.Length; i++) {
			spriteRenderers[i].sprite = images [index+i];
		}

		titleText.text = title;
		labelText.text = images [index].name;
	}
}
