using UnityEngine;
using System.Collections;

public class Player : Mover {

	public int distToTarget = 2;

	private int[] lastEndPos;
	private Mover target;

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			target = getTarget();

			Vector3 endPos;

			if(target == null) {
				endPos = getMousePosition ();
				findPath(endPos);
			}
		}

		if (target != null) {
			findPathToUnit();
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

	private void findPathToUnit() {
		int[] end = Map.map.worldToMapPoint(target.transform.position);
		
		if (currentPath == null || lastEndPos == null || lastEndPos [0] != end [0] || lastEndPos [1] != end [1]) { 
			findPath(target.transform.position);
			
			lastEndPos = end;
		}
	}

	private void findPath(Vector3 endPos) {
		Vector3 startPos = Map.map.mapToWorldPoint(reservedPos[0], reservedPos[1]);

		currentPath = AStar.findPath(Map.map, startPos, endPos, target, distToTarget);
	}
}
