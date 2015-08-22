using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	public Transform debugCircle;
	internal Transform myDebugCircle;
	
	internal SteeringUtils steeringUtils;
	internal FollowPath followPath;
	
	internal int[] reservedPos;
	
	// Use this for initialization
	void Start () {
		steeringUtils = GetComponent<SteeringUtils> ();
		followPath = GetComponent<FollowPath> ();
		
		reservePosition(transform.position);
		
		myDebugCircle = Instantiate(debugCircle, transform.position, Quaternion.identity) as Transform;
	}
	
	internal bool reservePosition(Vector2 pos) {
		int[] mapPos = Map.map.worldToMapPoint(pos);
		
		if(Map.map.objs[mapPos[0], mapPos[1]] == null) {
			Map.map.objs[mapPos[0], mapPos[1]] = this;
			reservedPos = mapPos;
			
			return true;
		}
		
		return false;
	}

}
