using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public float initialVelocity = 6f;
	
	
	private Rigidbody2D rb;
	private bool ballInPlay;
	
	void Awake () {
		
		rb = GetComponent<Rigidbody2D>();
		ballInPlay = false;
		
	}
	
	void Update () {
		//print (rb.velocity);

		if (Input.GetButtonDown("Fire1") && ballInPlay == false)
		{
			//transform.parent = null;
			ballInPlay = true;
			rb.isKinematic = false;
			rb.velocity = new Vector2(0, -initialVelocity);
		}
	}
}
