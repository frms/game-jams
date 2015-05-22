using UnityEngine;
using System.Collections;
using UnityEditor;

public class CameraMovement : MonoBehaviour {
	public float speed = 6;

	public float colorSpeed = 0.005f;

	private Camera cam;

	void Start() {
		cam = GetComponent<Camera> ();
	}

	void Update () {
		if (Input.GetButton ("Jump")) {
			transform.position += Vector3.up * speed * Time.deltaTime;

			float h, s, v;
			
			EditorGUIUtility.RGBToHSV(cam.backgroundColor, out h, out s, out v);
			
			h += colorSpeed * Time.deltaTime;
			
			if (h > 1) {
				h -= 1;
			}
			
			cam.backgroundColor = EditorGUIUtility.HSVToRGB (h, s, v);
		}
	}
}
