using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float speed = 5f;

	public Transform arrow;
	public Transform spawnPoint;

	private Rigidbody rb;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		// A Button
		if (Input.GetButtonDown ("Fire1")) {
			Transform clone = Instantiate(arrow, spawnPoint.position, transform.rotation) as Transform;

			Rigidbody cloneRb = clone.GetComponent<Rigidbody>();
			cloneRb.velocity = clone.right * speed;
		}
	}

	void FixedUpdate () {
		updateMoveCharacter ();	
	}

	private void updateMoveCharacter() {
		Vector2 stickDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		
		if(stickDirection.sqrMagnitude > 0.031) {
			
			// Rotate the character
			float moveAngle = Mathf.Atan2(stickDirection.y, stickDirection.x)*Mathf.Rad2Deg;
			if(moveAngle < 0) {
				moveAngle += 360;
			}

			moveAngle = getDirection(moveAngle);
			
			transform.rotation = Quaternion.Euler(0, moveAngle, 0);
			rb.velocity = transform.right * speed;
		} else {
			rb.velocity = Vector3.zero;
		}
	}
	
	private float getDirection(float moveAngle) {
		float dir = 315;

		if(moveAngle < 22.5 || moveAngle >= 337.5) {
			// Face right
			dir = 0;
		} else if(moveAngle < 67.5 && moveAngle >= 22.5) {
			// Face up right
			dir = 45;
		} else if(moveAngle < 112.5 && moveAngle >= 67.5) {
			// Face up
			dir = 90;
		} else if(moveAngle < 157.5 && moveAngle >= 112.5) {
			// Face up left
			dir = 135;
		} else if(moveAngle < 202.5 && moveAngle >= 157.5) {
			// Face left
			dir = 180;
		} else if(moveAngle < 247.5 && moveAngle >= 202.5) {
			// Face down left
			dir = 225;
		} else if(moveAngle < 292.5 && moveAngle >= 247.5) {
			// Face down
			dir = 270;
		} else if(moveAngle < 337.5 || moveAngle >= 292.5) {
			// Face down right
			dir = 315;
		}

		/* The above direction value was based on a counterclockwise unit circle.
		   But we want to rotate clockwise in this case. */
		dir = 360 - dir;

		return dir;
	}
}
