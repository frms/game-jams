using UnityEngine;
using System.Collections;

public class Troop : MonoBehaviour {
	public float speed = 0.5f;

	public float startTime = -1;
	public Vector3 target;

	private Vector3 startPos;
	private float maxDist;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		maxDist = Vector3.Distance (startPos, target);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > startTime) {
			float dt = Time.time - startTime;
			float t = (speed * dt) / maxDist;

			transform.position = Vector3.Lerp(startPos, target, t);

			if(t > 1 ) {
				Destroy(gameObject);
			}
		}
	}
}
