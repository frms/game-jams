using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float speed = 5f;

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
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast (r, out hit, Mathf.Infinity, groundLayer)) {
			//Debug.Log(hit.point);

			Vector3 dir = hit.point - transform.position;
			dir.Normalize();

			float angle = limitAngle(Mathf.Atan2(dir.z, dir.x)*Mathf.Rad2Deg);

			Debug.Log (angle);

			// A Button
			if (Input.GetButtonDown ("Fire1")) {
				Transform clone = Instantiate(arrow, spawnPoint.position, transform.rotation) as Transform;
				
				Rigidbody cloneRb = clone.GetComponent<Rigidbody>();
				cloneRb.velocity = clone.right * speed;
			}
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

			moveAngle = limitAngle(moveAngle);

			/* The above direction value was based on a counterclockwise unit circle.
			But we want to rotate clockwise in this case. */
			moveAngle = 360 - moveAngle;
			
			transform.rotation = Quaternion.Euler(0, moveAngle, 0);

			rb.velocity = transform.right * speed;
		} else {
			rb.velocity = Vector3.zero;
		}
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
