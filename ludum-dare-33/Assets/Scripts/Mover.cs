using UnityEngine;
using System.Collections;

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
		Vector2 accel;
		
		if (currentPath != null) {
			currentPath.draw ();
			
			Vector2 targetPosition;
			accel = followPath.getSteering (currentPath, pathLoop, out targetPosition);
			myDebugCircle.position = targetPosition;
			
			int[] mapPos = Map.map.worldToMapPoint (targetPosition);
			
			if (!equals (mapPos, reservedPos) && Map.map.objs [mapPos [0], mapPos [1]] == null) {
				Map.map.objs [reservedPos [0], reservedPos [1]] = null;
				Map.map.objs [mapPos [0], mapPos [1]] = this;
				reservedPos = mapPos;
			}
		}
		else {
			accel = steeringUtils.arrive (Map.map.mapToWorldPoint (reservedPos [0], reservedPos [1]));
		}
		
		steeringUtils.steer (accel);
		steeringUtils.lookWhereYoureGoing ();
	}

}
