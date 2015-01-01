using UnityEngine;
using System.Collections;

public class CreatorController : MonoBehaviour {

	public GameObject[] wrappers;

	// Use this for initialization
	void Start () {
		Instantiate (wrappers [0]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
