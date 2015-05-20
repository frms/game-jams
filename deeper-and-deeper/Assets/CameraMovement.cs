using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float speed = 6;

	void Update () {
		if (Input.GetButton ("Jump")) {
			transform.position += Vector3.up * speed * Time.deltaTime;
		}
	}
}
