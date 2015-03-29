using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour {
	public float visibleTime = 0.6f;
	public float fadeTime = 0.4f;

	private float timer;
	private SpriteRenderer sr;
	private Color start;
	private Color transparent;

	// Use this for initialization
	void Start () {
		timer = visibleTime;
		sr = GetComponent<SpriteRenderer> ();
		start = sr.color;
		transparent = start;
		transparent.a = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0) {
			if (timer <= fadeTime) { 
				sr.color = Color.Lerp (start, transparent, 1 - timer / fadeTime);
			}
			
			timer -= Time.deltaTime;
		} else {
			Destroy(transform.parent.gameObject);
		}
	}
	private static int count = 0;

	void OnTriggerEnter2D(Collider2D other) {
		count++;
		Debug.Log ("Beam Count: " + count);
	}
}
