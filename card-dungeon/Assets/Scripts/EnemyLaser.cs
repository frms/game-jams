using UnityEngine;
using System.Collections;

public class EnemyLaser : MonoBehaviour {
	private LineRenderer gunLine;

	// Use this for initialization
	void Start () {
		gunLine = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void fire(Vector3 target) {
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		gunLine.SetPosition (1, target);
	}

	public void stop() {
		gunLine.enabled = false;
	}
}
