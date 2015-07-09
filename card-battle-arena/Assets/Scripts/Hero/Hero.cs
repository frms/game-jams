using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {
	private MapData map;

	private SteeringUtils steeringUtils;
	private FollowPath followPath;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		map = GameObject.Find ("TileMap").GetComponent<MapBuilder> ().map;

		steeringUtils = GetComponent<SteeringUtils> ();
		followPath = GetComponent<FollowPath> ();
		rb = GetComponent<Rigidbody2D> ();
	}

	private LinePath currentPath;
	private Base atkTarget;

	// Update is called once per frame
	void Update () {
		// Right Click
		if (Input.GetMouseButtonDown (1)) {
			int[] start = map.worldToMapPoint(transform.position);
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int[] end = map.worldToMapPoint(worldPoint);

			Base target = map.objs [end [0], end [1]];
			
			currentPath = AStar.findPath (map, start, end, target);

			if(target != null && target.team != Base.TEAM_1) {
				atkTarget = target;
			} else {
				atkTarget = null;
			}
		}

		if(currentPath != null) {
			if(atkTarget != null && isAtEndOfPath ()) {
				steeringUtils.lookAtDirection(atkTarget.transform.position - transform.position);
				rb.velocity = Vector2.zero;
			} else {
				moveHero ();
			}
		}
		// If we have not path to the player then stand still and clear
		// any atk target we might have.
		else {
			rb.velocity = Vector2.zero;
			atkTarget = null;
		}
	}

	bool isAtEndOfPath ()
	{
		return Vector3.Distance (currentPath.endNode, transform.position) < followPath.stopRadius;
	}

	void moveHero ()
	{
		Vector2 accel = followPath.getSteering (currentPath);
		steeringUtils.steer (accel);
		steeringUtils.lookWhereYoureGoing ();
		currentPath.draw ();
	}
}
