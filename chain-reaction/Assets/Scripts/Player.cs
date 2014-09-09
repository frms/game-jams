using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public float speed = 5;

	public GameObject meleeImage;
	public float meleeArc = 180;
	public float meleeTime = 1;
	private float nextMelee = 0.0F;

	public int numOfElementA = 0;
	public int numOfElementB = 0;
	public int numOfElementC = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 stickDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		//Debug.Log (stickDirection);
		
		if(stickDirection.sqrMagnitude > 0.031) {
			// Move the character
			rigidbody2D.velocity = transform.right * speed;

			// Rotate the character
			float moveAngle = Mathf.Atan2(stickDirection.y, stickDirection.x)*Mathf.Rad2Deg;
			if(moveAngle < 0) {
				moveAngle += 360;
			}
			
			//Debug.Log (moveAngle);
			
			float rotation = moveAngle;
			//float rotation = getDirection(moveAngle);
			transform.rotation = Quaternion.Euler(0, 0, rotation);
		} else {
			rigidbody2D.velocity = Vector2.zero;
		}

		updateMeleeAttack ();
	}
	
	private float getDirection(float moveAngle) {
		if(moveAngle < 22.5 || moveAngle >= 337.5) {
			// Face right
			return 0;
		} else if(moveAngle < 67.5 && moveAngle >= 22.5) {
			// Face up right
			return 45;
		} else if(moveAngle < 112.5 && moveAngle >= 67.5) {
			// Face up
			return 90;
		} else if(moveAngle < 157.5 && moveAngle >= 112.5) {
			// Face up left
			return 135;
		} else if(moveAngle < 202.5 && moveAngle >= 157.5) {
			// Face left
			return 180;
		} else if(moveAngle < 247.5 && moveAngle >= 202.5) {
			// Face down left
			return 225;
		} else if(moveAngle < 292.5 && moveAngle >= 247.5) {
			// Face down
			return 270;
		} else if(moveAngle < 337.5 || moveAngle >= 292.5) {
			// Face down right
			return 315;
		}
		
		Debug.Log ("SHOULD NOT BE REACHING THIS CODE");
		return 315;
	}

	private void updateMeleeAttack() {
		// If melee button is hit and not already in a melee attack then start attacking
		if(Input.GetButtonDown("Fire1") && Time.time > nextMelee) {
			nextMelee = Time.time + meleeTime;
		}

		// If we are still melee attacking then animate it
		if (nextMelee - Time.time >= 0) {
			float angle = meleeArc * (1 - (nextMelee - Time.time) / meleeTime);
			angle -= meleeArc/2f;
			
			Vector2 position = new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad));
			meleeImage.transform.localPosition = position*0.6f;
			meleeImage.transform.localRotation = Quaternion.Euler(0, 0, angle);
			
			meleeImage.SetActive(true);
		}
		// Else hide the attack image
		else {
			meleeImage.SetActive(false);
		}
	}

	public void gainElementA() {
		numOfElementA++;
	}

	public void gainElementB() {
		numOfElementB++;
	}

	public void gainElementC() {
		numOfElementC++;
	}
}
