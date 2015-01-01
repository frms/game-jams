using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Step : MonoBehaviour {
	public string title;
	public Sprite[] images;
	private int index;
	private SpriteRenderer spriteRenderer;

	private Text titleText;
	private Text labelText;

	// Use this for initialization
	void Awake () {
		titleText = GameObject.Find ("title").GetComponent<Text> ();
		labelText = GameObject.Find ("label").GetComponent<Text> ();
	}

	public void setUp(string path) {
		title = path;
		images = Resources.LoadAll<Sprite>(path);

		index = 0;
		GameObject imgGameObj = new GameObject (title);
		spriteRenderer = imgGameObj.AddComponent<SpriteRenderer> ();
	}
	
	public void next() {
		index++;
		index %= images.Length;
		showCurrent();
	}
	
	public void prev() {
		index--;
		index = (index < 0) ? images.Length - 1 : index;
		showCurrent ();
	}
	
	/**
	 * Shows the current prefab and returns the current prefab's name.
	 */
	public void showCurrent() {
		spriteRenderer.sprite = images [index];

		titleText.text = title;
		labelText.text = images [index].name;
	}
}
