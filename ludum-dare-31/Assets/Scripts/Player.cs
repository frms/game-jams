using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public float speed = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		updateMoveCharacter();
	}

	private void updateMoveCharacter() {
		Vector2 stickDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		
		//stickDirection = stickDirection.normalized;
		
		if(stickDirection.sqrMagnitude > 0.031) {
			
			// Rotate the character
			float moveAngle = Mathf.Atan2(stickDirection.y, stickDirection.x)*Mathf.Rad2Deg;
			if(moveAngle < 0) {
				moveAngle += 360;
			}
			
			//Debug.Log (moveAngle);

			transform.rotation = Quaternion.Euler(0, -1*moveAngle, 0);
			rigidbody.velocity = transform.right * speed;
		} else {
			rigidbody.velocity = Vector3.zero;
		}
	}

}
