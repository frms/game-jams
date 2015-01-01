using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreatorController : MonoBehaviour {
	public Text label;

	public GameObject[] wrappers;

	private int wrapperIndex = 0;

	private GameObject wrapperShown;

	// Use this for initialization
	void Start () {
		showCurrentWrapper ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void next() {
		wrapperIndex++;
		wrapperIndex %= wrappers.Length;
		showCurrentWrapper ();
	}

	public void prev() {
		wrapperIndex--;
		wrapperIndex = (wrapperIndex < 0) ? wrappers.Length - 1 : wrapperIndex;
		showCurrentWrapper ();
	}

	private void showCurrentWrapper() {
		Destroy (wrapperShown);
		wrapperShown = Instantiate (wrappers [wrapperIndex]) as GameObject;
		label.text = wrappers [wrapperIndex].name;
	}
}
