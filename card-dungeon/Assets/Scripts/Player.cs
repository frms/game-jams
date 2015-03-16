using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 5f;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
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

			transform.rotation = Quaternion.Euler(0, 0, moveAngle);
			rb.velocity = transform.right * speed;
			
		} else {
			rb.velocity = Vector2.zero;
		}
	}
}
