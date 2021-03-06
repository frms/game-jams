﻿using UnityEngine;
using System.Collections;

public class PlayerRotate : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 stickDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		//Debug.Log (stickDirection);
		
		if(stickDirection.sqrMagnitude > 0.031) {
			float moveAngle = Mathf.Atan2(stickDirection.y, stickDirection.x)*Mathf.Rad2Deg;
			if(moveAngle < 0) {
				moveAngle += 360;
			}
			
			//Debug.Log (moveAngle);

			float rotation = getDirection(moveAngle);
			transform.rotation = Quaternion.Euler(0, 0, rotation);
		}
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
}
