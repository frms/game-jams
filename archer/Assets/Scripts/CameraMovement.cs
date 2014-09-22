using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Player").transform;
	}
	
	// LateUpdate is called once per frame after the other normal Update functions have already run
	void LateUpdate () {
		//Debug.Log (Vector2.Distance (transform.position, target.position));

		transform.position = target.position + new Vector3 (0, 0, -10.0f);
	}
}
