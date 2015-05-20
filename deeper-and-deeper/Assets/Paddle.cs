using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {
	
	public float paddleSpeed = 1f;

	public float edgeReboundAngle = 45;

	private float adjacent;

	void Start() {
		adjacent = transform.localScale.x / 2;
	}
	
	void Update () 
	{
		float xPos = transform.position.x + (Input.GetAxisRaw("Horizontal") * paddleSpeed * Time.deltaTime);
		Vector3 playerPos = new Vector3 (Mathf.Clamp (xPos, -8f, 8f), transform.position.y, transform.position.z);
		transform.position = playerPos;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Vector2 aveContactPoint = getContactPoint(coll);

		float opposite = aveContactPoint.x - transform.position.x;

		float reboundAngle = Mathf.Atan (opposite / adjacent);

		reboundAngle = Mathf.PI / 2 + -1 * reboundAngle;

		Vector2 reboundDir = new Vector2 (Mathf.Cos (reboundAngle), Mathf.Sin (reboundAngle));

		Rigidbody2D rb = coll.gameObject.GetComponent<Rigidbody2D> ();
		rb.velocity = reboundDir * 6;
	}

	private Vector2 getContactPoint(Collision2D coll) {
		Vector2 aveContactPoint = Vector2.zero;
		
		for (int i = 0; i < coll.contacts.Length; i++) {
			aveContactPoint += coll.contacts[i].point;
		}
		
		aveContactPoint /= coll.contacts.Length;

		return aveContactPoint;
	}
}
