using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	private GameCanvas canvas;
	
	// Use this for initialization
	void Start () {
		canvas = GameObject.Find ("GameCanvas").GetComponent<GameCanvas> ();
	}
	
	// Update is called once per frame
	void Update () {
		canvas.drawColor (transform.position, Color.red);
	}
}
