using UnityEngine;
using System.Collections;

public class Player : Mover {

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			target = getTarget();

			Vector3 endPos;

			if(target == null) {
				endPos = getMousePosition ();

				findPath(endPos);
				lastEndPos = Map.map.worldToMapPoint(endPos);
			}
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
