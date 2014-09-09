using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	
	public float maxDistance;
	public float damage;
	
	public Vector2 startPoint;
	
	void Awake() {
		// Call set up with the default arrow properties
		this.setUp (new ArrowProperties ());
	}
	
	public void setUp(ArrowProperties properties) {
		maxDistance = properties.maxDistance;
		damage = properties.damage;

		startPoint = transform.position;
	}

	void OnCollisionEnter2D(Collision2D other) {
		//Debug.Log ("On Collision " + other.gameObject.name);

		if (other.gameObject.tag == "Enemy") {
			other.gameObject.SendMessage("applyDamage", damage);
		}

		stopFlight ();
	}
	
	
	void Update() {
		if (Vector2.Distance (startPoint, transform.position) > maxDistance) {
			stopFlight();
		}
	}

	private void stopFlight() {
		Destroy(gameObject);
	}
}

public class ArrowProperties {

	public float maxDistance;
	public float damage;
	
	public ArrowProperties() {
		maxDistance = Mathf.Infinity;
		damage = 10;
	}
}
