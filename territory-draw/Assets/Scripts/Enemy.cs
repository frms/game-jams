using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	private GameCanvas canvas;
	private Color color;
	
	// Use this for initialization
	void Start () {
		canvas = GameObject.Find ("GameCanvas").GetComponent<GameCanvas> ();
		color = new Color ((178f / 255f), (143f / 255f), (204f / 255f));
	}
	
	// Update is called once per frame
	void Update () {
		canvas.drawColor (transform.position, color);
	}
}
