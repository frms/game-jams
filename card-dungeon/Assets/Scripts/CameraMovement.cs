using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public Transform target;
	private Vector3 offset;

	// Use this for initialization
	void Awake () {
		target = GameObject.Find ("Player").transform;
		offset = transform.position - target.position;
	}
	
	// LateUpdate is called once per frame after the other normal Update functions have already run
	void LateUpdate () {
		transform.position = target.position + offset;
	}
}
