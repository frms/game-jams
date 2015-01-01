using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Step : MonoBehaviour {
	public string title;
	public GameObject[] prefabs;
	private GameObject prefabShown;

	private int prefabIndex = 0;

	private Text titleText;
	private Text labelText;

	// Use this for initialization
	void Awake () {
		titleText = GameObject.Find ("title").GetComponent<Text> ();
		labelText = GameObject.Find ("label").GetComponent<Text> ();
	}

	public void setUp(StepInspector si) {
		title = si.title;
		prefabs = si.prefabs;
	}
	
	public void next() {
		prefabIndex++;
		prefabIndex %= prefabs.Length;
		showCurrent();
	}
	
	public void prev() {
		prefabIndex--;
		prefabIndex = (prefabIndex < 0) ? prefabs.Length - 1 : prefabIndex;
		showCurrent ();
	}
	
	/**
	 * Shows the current prefab and returns the current prefab's name.
	 */
	public void showCurrent() {
		Destroy (prefabShown);
		prefabShown = Instantiate (prefabs [prefabIndex]) as GameObject;

		titleText.text = title;
		labelText.text = prefabs [prefabIndex].name;
	}
}
