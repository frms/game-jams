using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
	public float distance = 20f;

	private LineRenderer gunLine;
	private Transform user;
	
	// Use this for initialization
	void Start () {
		gunLine = GetComponent<LineRenderer> ();
		user = transform.parent;
	}
	
	public void use() {
		Vector3 endPoint = transform.position + transform.right.normalized * distance;

		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		gunLine.SetPosition (1, endPoint);
	}
}
