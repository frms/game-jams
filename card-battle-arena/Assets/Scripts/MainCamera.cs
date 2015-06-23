using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TileMap tm = GameObject.Find ("TileMap").GetComponent<TileMap> ();

		float w = tm.mapWidth * tm.tileSize;
		float h = tm.mapHeight * tm.tileSize;

		Camera camera = GetComponent<Camera> ();

		if (w / h < camera.aspect) {
			camera.orthographicSize = h / 2f;
		} else {
			camera.orthographicSize = w / (2f * camera.aspect);
		}

		Vector3 tmPos = tm.transform.position;

		Vector3 camPos = new Vector3 (tmPos.x, tmPos.y, transform.position.z);
		camPos.x += w / 2f;
		camPos.y += h / 2f;

		transform.position = camPos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
