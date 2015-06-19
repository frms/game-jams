using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float speed = 5f;
	public float spawnDist = 1.1f;

	public Transform arrow;

	private Rigidbody rb;
	private int groundLayer;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

		groundLayer = 1 << LayerMask.NameToLayer ("Ground");
	}
	
	// Update is called once per frame
	void Update () {
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast (r, out hit, Mathf.Infinity, groundLayer)) {
			// A Button
			if (Input.GetButtonDown ("Fire1")) {
				createArrow(hit);
			}
		}
	}

	public void createArrow(RaycastHit hit) {
		Vector3 dir = hit.point - transform.position;
		
		float angle = limitAndInvertAngle (dir.x, dir.z);
		
		float angleCCW = (360 - angle) * Mathf.Deg2Rad;
		
		dir = new Vector3 (Mathf.Cos (angleCCW), 0, Mathf.Sin (angleCCW));
		
		Vector3 spawnPoint = transform.position + (dir * spawnDist);
		
		Transform clone = Instantiate (arrow, spawnPoint, Quaternion.Euler (0, angle, 0)) as Transform;
		
		Rigidbody cloneRb = clone.GetComponent<Rigidbody> ();
		cloneRb.velocity = clone.right * speed;
	}
		
	void FixedUpdate () {
		updateMoveCharacter ();	
	}

	private void updateMoveCharacter() {
		Vector2 stickDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		
		if(stickDirection.sqrMagnitude > 0.031) {
			// Rotate the character
			float moveAngle = limitAndInvertAngle(stickDirection.x, stickDirection.y);
			
			transform.rotation = Quaternion.Euler(0, moveAngle, 0);

			rb.velocity = transform.right * speed;
		} else {
			rb.velocity = Vector3.zero;
		}
	}

	private float limitAndInvertAngle(float x, float y) {
		float angle = Mathf.Atan2 (y, x) * Mathf.Rad2Deg;

		return limitAndInvertAngle (angle);
	}

	private float limitAndInvertAngle(float angle) {
		float dir = limitAngle (angle);

		/* Invert the angle so that if the input angle was Clockwise then the output will be 
		 * Counterclockwise and vice versa */
		dir = 360 - dir;

		return dir;
	}

	private float limitAngle(float angle) {
		if(angle < 0) {
			angle += 360;
		}
		
		float dir = 315;

		if(angle < 22.5 || angle >= 337.5) {
			// Face right
			dir = 0;
		} else if(angle < 67.5 && angle >= 22.5) {
			// Face up right
			dir = 45;
		} else if(angle < 112.5 && angle >= 67.5) {
			// Face up
			dir = 90;
		} else if(angle < 157.5 && angle >= 112.5) {
			// Face up left
			dir = 135;
		} else if(angle < 202.5 && angle >= 157.5) {
			// Face left
			dir = 180;
		} else if(angle < 247.5 && angle >= 202.5) {
			// Face down left
			dir = 225;
		} else if(angle < 292.5 && angle >= 247.5) {
			// Face down
			dir = 270;
		} else if(angle < 337.5 || angle >= 292.5) {
			// Face down right
			dir = 315;
		}

		return dir;
	}
}
