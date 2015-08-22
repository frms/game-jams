using UnityEngine;
using System.Collections;

public class Player : Mover {

	private LinePath currentPath;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			Vector3 startPos = Map.map.mapToWorldPoint(reservedPos[0], reservedPos[1]);
			Vector3 endPos = getMousePosition ();

			currentPath = AStar.findPath(Map.map, startPos, endPos, null, false);
		}

		Vector2 accel;

		if(currentPath != null) {
			currentPath.draw();

			Vector2 targetPosition;
			accel = followPath.getSteering (currentPath, false, out targetPosition);
			myDebugCircle.position = targetPosition;

			int[] mapPos = Map.map.worldToMapPoint(targetPosition);
			if(!equals(mapPos, reservedPos) && Map.map.objs[mapPos[0], mapPos[1]] == null) {
				Map.map.objs[reservedPos[0], reservedPos[1]] = null;

				Map.map.objs[mapPos[0], mapPos[1]] = this;
				reservedPos = mapPos;
			}
		} else {
			accel = steeringUtils.arrive(Map.map.mapToWorldPoint(reservedPos[0], reservedPos[1]));
		}

		steeringUtils.steer (accel);
		steeringUtils.lookWhereYoureGoing ();
	}

	private Vector3 getMousePosition () {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -1 * Camera.main.transform.position.z;

		return Camera.main.ScreenToWorldPoint (mousePos);
	}
}
