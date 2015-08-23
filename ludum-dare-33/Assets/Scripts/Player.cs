using UnityEngine;
using System.Collections;

public class Player : Mover {

	public int distToTarget = 2;

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			Mover target = getTarget();

			Vector3 startPos = Map.map.mapToWorldPoint(reservedPos[0], reservedPos[1]);
			Vector3 endPos;

			if(target == null) {
				endPos = getMousePosition ();
			} else {
				int[] coords = Map.map.worldToMapPoint(target.transform.position);
				endPos = Map.map.mapToWorldPoint(coords[0], coords[1]);
			}

			currentPath = AStar.findPath(Map.map, startPos, endPos, target, distToTarget);
		}

		moveUnit ();
	}

	private Mover getTarget() {
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(r, out hit, Mathf.Infinity, GameManager.enemyMask)) {
			return hit.transform.GetComponent<Mover>();
		}

		return null;
	}

	private Vector3 getMousePosition () {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -1 * Camera.main.transform.position.z;

		return Camera.main.ScreenToWorldPoint (mousePos);
	}
}
