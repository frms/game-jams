using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)) {
			anim.SetBool("moving", true);
		} else {
			anim.SetBool("moving", false);
		}

	}

// How to use just the moving animation and start and stop it, rather than idle and moving
//	void Update () {
//		anim.SetBool("moving", true);
//		
//		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)) {
//			anim.speed = 1;
//		} else {
//			anim.speed = 0;
//		}
//	}
}
