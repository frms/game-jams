using UnityEngine;
using System.Collections;

public class WallAvoid : MonoBehaviour {

	public float rayDistance = 1.5f;
	public float sameDecisionDuration = 3;

	private float lastDecisionTime = Mathf.NegativeInfinity;
	private int decision;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 dir = orientationToVector (transform.rotation.eulerAngles.z * Mathf.Deg2Rad);

		Vector3 end = transform.position + dir * rayDistance;
		Debug.DrawLine (transform.position, end, Color.green);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayDistance);
		if (hit.collider != null) {
			getDirection();
		}
	}

	/* Returns the orientation as a unit vector */
	Vector3 orientationToVector(float orientation) {
		return new Vector3(Mathf.Cos(orientation), Mathf.Sin(orientation), 0);
	}

	private int getDirection() {
		if(lastDecisionTime + sameDecisionDuration < Time.time) {
			decision = (int)(Random.value * 2);
			lastDecisionTime = Time.time;

			Debug.Log (decision);
		}
		return decision;
	}
}
