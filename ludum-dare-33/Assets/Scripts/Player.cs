using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private SteeringUtils steeringUtils;
	private FollowPath followPath;

	// Use this for initialization
	void Start () {
		steeringUtils = GetComponent<SteeringUtils> ();
		followPath = GetComponent<FollowPath> ();
	}

	private LinePath currentPath;

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			Vector3 endPos = getMousePosition ();

			currentPath = AStar.findPath(Map.map, transform.position, endPos, null);
		}

		if(currentPath != null) {
			currentPath.draw();

			Vector2 accel = followPath.getSteering (currentPath, false);
			
			steeringUtils.steer (accel);
			steeringUtils.lookWhereYoureGoing ();
		}
	}

	private Vector3 getMousePosition () {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -1 * Camera.main.transform.position.z;

		return Camera.main.ScreenToWorldPoint (mousePos);
	}
}
