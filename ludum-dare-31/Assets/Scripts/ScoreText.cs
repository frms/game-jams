using UnityEngine;
using System.Collections;

public class ScoreText : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	private float endOfLife = -1;
	private float lifeDuration = 1;
	
	void Update() {
		if(endOfLife == -1) {
			endOfLife = Time.time + lifeDuration;
		}
		
		if(endOfLife < Time.time) {
			Destroy(gameObject);
		}
	}
}
