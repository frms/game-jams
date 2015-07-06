using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {
	private MapData map;

	private SteeringUtils steeringUtils;
	private FollowPath followPath;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		map = GameObject.Find ("TileMap").GetComponent<MapBuilder> ().map;

		steeringUtils = GetComponent<SteeringUtils> ();
		followPath = GetComponent<FollowPath> ();
		rb = GetComponent<Rigidbody2D> ();

		Debug.Log (map.worldToMapPoint(transform.position)[0] + " " + map.worldToMapPoint(transform.position)[1]);
	}

	private LinePath currentPath;

	// Update is called once per frame
	void Update () {
		// Right Click
		if (Input.GetMouseButtonDown (1)) {
			int[] start = map.worldToMapPoint(transform.position);
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int[] end = map.worldToMapPoint(worldPoint);
			
			currentPath = AStar.findPath (map, start, end);
		}

		if(currentPath != null) {
			Vector2 accel = followPath.getSteering (currentPath);
			steeringUtils.steer (accel);
			steeringUtils.lookWhereYoureGoing ();
			
			currentPath.draw();
		}
		// If we have not path to the player then stand still
		else {
			rb.velocity = Vector2.zero;
		}
	}
}
