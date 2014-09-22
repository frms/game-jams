using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	public int id;

	public float maxDistance;

	public bool inFlight;

	public Vector2 startPoint;

	void Awake() {
		// Call set up with the default arrow properties
		this.setUp (new ArrowProperties ());
	}

	public void setUp(ArrowProperties properties) {
		id = properties.id;
		maxDistance = properties.maxDistance;
		collider2D.isTrigger = properties.isTrigger;

		inFlight = true;
		startPoint = transform.position;
	}

	public void stopFlight() {
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.isKinematic = true;
		collider2D.isTrigger = true;
		
		inFlight = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("On Trigger " + other.gameObject.name);
	}

	void OnCollisionEnter2D(Collision2D other) {
		//Debug.Log ("On Collision " + other.gameObject.name);
		stopFlight ();

		transform.parent = other.transform;
	}
	

	void Update() {
		if (inFlight) {
			if (Vector2.Distance (startPoint, transform.position) > maxDistance) {
				stopFlight();
			}
		}
	}
}

public class ArrowProperties {
	public int id;
	public float maxDistance;
	public bool isTrigger;

	public ArrowProperties() {
		id = -1;
		maxDistance = Mathf.Infinity;
		isTrigger = false;
	}
}
