using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public float speed;

	public GameObject attack;

	private float attackStart = -1;
	
	// Use this for initialization
	void Start () {
		attack.SetActive(false);
	}

	void Update() {
		// If we aren't attacking
		if (attackStart == -1) {
			// And if space is pressed then start attacking
			if (Input.GetKeyDown (KeyCode.Space)) {
				attack.SetActive(true);
				attackStart = Time.time;
			}
		}
		// Else we are attacking
		else {
			// If we have been attacking for long enough then stop
			if (Time.time > attackStart + 1) {
				attack.SetActive(false);
				attackStart = -1;
			}
		}
	}

	// Update is called once per physics frame
	void FixedUpdate () {
		Vector2 direction = Vector2.zero;
		
		if (Input.GetKey (KeyCode.W)) {
			direction += Vector2.up;
		}
		if (Input.GetKey (KeyCode.A)) {
			direction += -1 * Vector2.right;
		}
		if (Input.GetKey (KeyCode.S)) {
			direction += -1 * Vector2.up;
		}
		if (Input.GetKey (KeyCode.D)) {
			direction += Vector2.right;
		}
		
		direction.Normalize();
		
		// Set the rigid body velocity with the given world space direction and speed
		rigidbody2D.velocity = direction * speed;

		// Set the rotation of the player only if we have key inputs (aka direction is non-zero) otherwise
		// leave the rotation alone
		if (direction.sqrMagnitude > 0.001) {
			float rotation = (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg) + 90f;
			transform.rotation = Quaternion.Euler(0, 0, rotation);
		}
		
	}
	
}
