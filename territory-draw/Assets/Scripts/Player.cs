using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 5;
	public bool isDrawing = true;
	public int colorIndex = 0;

	private Color[] colors;

	private GameCanvas canvas;

	private bool gameOver = false;

	// Use this for initialization
	void Start () {
		colors = new [] { Color.blue, Color.red };

		canvas = GameObject.Find ("GameCanvas").GetComponent<GameCanvas> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameOver) {
			rigidbody2D.velocity = Vector2.zero;
			return;
		}

		updateMoveCharacter ();

//		if (Input.GetButtonDown ("Fire2")) {
//			colorIndex++;
//			colorIndex = colorIndex % colors.Length;
//		} else if (Input.GetButtonDown ("Fire3")) {
//			isDrawing = !isDrawing;
//		}

		if(isDrawing) {
			canvas.drawColor (transform.position, colors[colorIndex]);
		}
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
			//moveAngle = getDirection(moveAngle);
			
			// If the orientation of the character has changed then update it, but don't move the character yet
			// because Unity has a minor bug where sometimes when you move and change orientation at the same
			// time you move partly in a direction you dont want to go.
			//if(transform.rotation != Quaternion.Euler(0, 0, moveAngle)) {
				transform.rotation = Quaternion.Euler(0, 0, moveAngle);
			//}
			// Else we are facing the right orientation so move the character
			//else {
				rigidbody2D.velocity = transform.right * speed;
			//}
			
		} else {
			rigidbody2D.velocity = Vector2.zero;
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

	public void timeIsUp() {
		gameOver = true;
	}
}
