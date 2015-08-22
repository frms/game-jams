using UnityEngine;
using System.Collections;

public class Player : Mover {

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			Vector3 startPos = Map.map.mapToWorldPoint(reservedPos[0], reservedPos[1]);
			Vector3 endPos = getMousePosition ();

			currentPath = AStar.findPath(Map.map, startPos, endPos, null, false);
		}

		moveUnit (false);
	}

	private Vector3 getMousePosition () {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -1 * Camera.main.transform.position.z;

		return Camera.main.ScreenToWorldPoint (mousePos);
	}
}
