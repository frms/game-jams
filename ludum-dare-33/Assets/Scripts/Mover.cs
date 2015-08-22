using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Mover : MonoBehaviour {

	public Transform debugCircle;
	internal Transform myDebugCircle;
	
	internal SteeringUtils steeringUtils;
	internal FollowPath followPath;
	internal Rigidbody2D rb;
	
	internal int[] reservedPos;
	
	// Use this for initialization
	void Start () {
		steeringUtils = GetComponent<SteeringUtils> ();
		followPath = GetComponent<FollowPath> ();
		rb = GetComponent<Rigidbody2D>();

		int[] mapPos = Map.map.worldToMapPoint(transform.position);
		reservePosition(mapPos);
		
		myDebugCircle = Instantiate(debugCircle, transform.position, Quaternion.identity) as Transform;
	}
	
	internal bool reservePosition(int[] mapPos) {	
		if(Map.map.objs[mapPos[0], mapPos[1]] == null) {
			Map.map.objs[mapPos[0], mapPos[1]] = this;
			reservedPos = mapPos;
			
			return true;
		}
		
		return false;
	}

	internal static bool equals(int[] a, int[] b) {
		return (a == null && b == null) || (a != null && b != null && a[0] == b[0] && a[1] == b[1]);
	}

	internal LinePath currentPath;
	
	internal void moveUnit (bool pathLoop)
	{
		Vector2 accel = Vector2.zero;
	
		bool standStill = false;

		if (currentPath != null) {
			Vector2 targetPosition;

			accel = steerTowardsPath(pathLoop, out targetPosition);
			
			int[] mapPos = Map.map.worldToMapPoint (targetPosition);
			
			if (!equals (mapPos, reservedPos)) {
				if(Map.map.getObj(mapPos) == null) {
					Map.map.setObj(reservedPos, null);
					Map.map.setObj(mapPos, this);
					reservedPos = mapPos;
				} else {
					Debug.Log (name + " is moving onto an occupied node");

					int i = findNextUnoccupiedNode (targetPosition);

					// Should prob change the if to find path to target and get as close as possible
					// and make the else code happen outside of the else block
					if(i == currentPath.Length) {
						Debug.Log(name + " has no unoccupied nodes on its current path to its goal");
						standStill = true;
					} else {
						Vector3 startPos = Map.map.mapToWorldPoint(reservedPos[0], reservedPos[1]);
						Vector3 endPos = currentPath[i];

						LinePath detour = AStar.findPath(Map.map, startPos, endPos, null, false);

						/* If we can't find a detour path just find a way to the end node */
						if(detour == null) {
							Debug.Log (name + " no detour to next open space. Finding new path to end goal all together.");
							currentPath = AStar.findPath(Map.map, currentPath[0], currentPath.endNode, null, false);
						} 
						/* Else update the current path */
						else {
							//Vector3[] newNodes = new Vector3[detour.Length + (currentPath.Length - i - 1)];

							List<Vector3> newNodes = new List<Vector3>();

							for(int j = 0; j < currentPath.Length; j++) {
								if(currentPath[j] != startPos) {
									newNodes.Add(currentPath[j]);
									break;
								}
							}

							for(int j = 0; j < detour.Length; j++) {
								newNodes.Add(detour[j]);
							}

							i = i + 1;
							for(; i < currentPath.Length; i++) {
								newNodes.Add( currentPath[i]);
							}

							currentPath = new LinePath(newNodes.ToArray());
						}

						accel = steerTowardsPath(pathLoop, out targetPosition);
					}
				}
			}
		}
		else {
			standStill = true;
		}

		if (standStill) {
			accel = steeringUtils.arrive (Map.map.mapToWorldPoint (reservedPos [0], reservedPos [1]));
		}
		
		steeringUtils.steer (accel);
		steeringUtils.lookWhereYoureGoing ();
	}

	public int findNextUnoccupiedNode(Vector2 pos) {
		int i = currentPath.getClosestSegement (pos);
		
		for(; i < currentPath.Length; i++) {
			int index = i;

			if(followPath.pathDirection < 0) {
				index = currentPath.Length - 1 - i;
			}

			int[] mapCoords = Map.map.worldToMapPoint(currentPath[index]);
			
			if(Map.map.getObj(mapCoords) == null) {
				break;
			}
		}

		return i;
	}


	public Vector2 steerTowardsPath(bool pathLoop, out Vector2 targetPosition) {
		currentPath.draw ();

		Vector2 accel = followPath.getSteering (currentPath, pathLoop, out targetPosition);
		myDebugCircle.position = targetPosition;

		return accel;
	}
}
