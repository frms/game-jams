using UnityEngine;
using System.Collections;

public class Enemy1 : Mover {

	public bool isLooping;

	// Update is called once per frame
	void Update () {
		if(isLooping && currentPath != null && isAtEndOfPath()) {
			currentPath = AStar.findPath(Map.map, currentPath.endNode, currentPath[0], null);
		}

		moveUnit ();
	}

	bool isAtEndOfPath () {
		return Vector3.Distance (currentPath.endNode, transform.position) < followPath.stopRadius;
	}

}
