using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {
	private MapData map;

	// Use this for initialization
	void Start () {
		map = GameObject.Find ("TileMap").GetComponent<TileMap> ().mapBuilder.map;

		Debug.Log (map.worldToMapPoint(transform.position)[0] + " " + map.worldToMapPoint(transform.position)[1]);
	}
	
	// Update is called once per frame
	void Update () {
		// Right Click
		if (Input.GetMouseButtonDown (1)) {
			int[] start = map.worldToMapPoint(transform.position);
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int[] end = map.worldToMapPoint(worldPoint);

			Debug.Log ("HERE!");
			
			LinePath currentPath = AStar.findPath (map, start, end);
			currentPath.draw();
		}
	}
}
