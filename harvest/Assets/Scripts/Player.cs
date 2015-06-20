using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float speed = 5f;

	public float arrowSpeed = 10f;
	public float fireRate = 0.5F;

	private float nextFire = 0.0F;
	
	public Transform arrow;
	public Transform spawnPoint;

	private Rigidbody rb;
	private int groundLayer;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

		groundLayer = 1 << LayerMask.NameToLayer ("Ground");
	}
	
	// Update is called once per frame
	void Update () {
		Ray r = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		RaycastHit hit;
		if (!Physics.Raycast (r, out hit, Mathf.Infinity, groundLayer)) {
			return;
		}

		rotateCharacter (hit);

		// A Button
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			createArrow ();
		}

		// B Button
		if (Input.GetButtonDown ("Fire2")) {
			Debug.Log ("YO");
		}
	}

	void rotateCharacter (RaycastHit hit)
	{
		Vector3 dir = hit.point - transform.position;
		float angle = Mathf.Atan2 (dir.z, dir.x) * Mathf.Rad2Deg;
		angle = 360 - angle;
		transform.rotation = Quaternion.Euler (0, angle, 0);
	}

	public void createArrow() {
		Transform clone = Instantiate (arrow, spawnPoint.position, spawnPoint.rotation) as Transform;
		
		Rigidbody cloneRb = clone.GetComponent<Rigidbody> ();
		cloneRb.velocity = spawnPoint.right * arrowSpeed;
	}
		
	void FixedUpdate () {
		updateMoveCharacter ();	
	}

	private void updateMoveCharacter() {
		Vector2 stickDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		
		if(stickDirection.sqrMagnitude > 0.031) {
			float moveAngle = limitAngle(stickDirection.x, stickDirection.y);

			Vector3 vel = new Vector3();
			vel.x = Mathf.Cos(moveAngle*Mathf.Deg2Rad);
			vel.y = 0;
			vel.z = Mathf.Sin(moveAngle*Mathf.Deg2Rad);

			rb.velocity = vel * speed;
		} else {
			rb.velocity = Vector3.zero;
		}
	}

	private float limitAndInvertAngle(float x, float y) {
		float dir = limitAngle (x, y);

		/* Invert the angle so that if the input angle was Clockwise then the output will be 
		 * Counterclockwise and vice versa */
		dir = 360 - dir;

		return dir;
	}

	private float limitAngle(float x, float y) {
		float angle = Mathf.Atan2 (y, x) * Mathf.Rad2Deg;

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
