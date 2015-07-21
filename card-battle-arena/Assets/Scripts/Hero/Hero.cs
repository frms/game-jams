using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : TeamMember {
	public float atkRate = 0.5F;
	public float atkDmg = 5f;

	[System.NonSerialized]
	public bool playerControlled;

	[System.NonSerialized]
	public bool inSelectionBox;

	[System.NonSerialized]
	public LinePath patrolPath;

	private MapData map;

	private SteeringUtils steeringUtils;
	private FollowPath followPath;

	private Dictionary<Transform, Collision2D> touching = new Dictionary<Transform, Collision2D>();
	private int selectionBoxLayer;

	private NearSensor nearSensor;

	// Use this for initialization
	public override void Start () {
		base.Start();

		map = GameObject.Find ("TileMap").GetComponent<MapBuilder> ().map;

		steeringUtils = GetComponent<SteeringUtils> ();
		followPath = GetComponent<FollowPath> ();

		selectionBoxLayer = LayerMask.NameToLayer ("SelectionBox");

		nearSensor = GetComponentInChildren<NearSensor> ();
	}

	private int[] lastEndPos;
	private LinePath currentPath;
	private TeamMember target;
	private HealthBar enemyHealth;
	private float nextFire = 0.0F;

	// Update is called once per frame
	public override void Update () {
		if (playerControlled) {
			playerControlledUpdate ();
		} else if (patrolPath != null) {
			currentPath = patrolPath;
		}

		// Control highlight
		highlight.SetActive (playerControlled || mouseIsOver || inSelectionBox);
	}

	private void playerControlledUpdate() {
		// Right Click
		if (Input.GetMouseButtonDown (1)) {
			target = castRay();

			if(target == null) {
				Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				worldPoint.z = 0;
				lastEndPos = map.worldToMapPoint(worldPoint);
				
				target = map.objs [lastEndPos [0], lastEndPos [1]];

				currentPath = AStar.findPath (map, transform.position, worldPoint, target);
			}
			
			if(target != null && target.teamId != TeamMember.TEAM_1) {
				enemyHealth = target.GetComponent<HealthBar>();
			} else {
				enemyHealth = null;
			}
		}
	}

	private Hero castRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		if (hit) {
			return hit.transform.GetComponent<Hero>();
		}

		return null;
	}

	void FixedUpdate () {
		if (target != null && target is Hero) {
			findPathToHero();
		}

		// Follow path and atk target if its an enemy
		if (currentPath != null) {
			if (target != null && nearSensor.targets.Contains(target.transform)) {
				//Look at the target and stop moving
				steeringUtils.lookAtDirection (target.transform.position - transform.position);
				steeringUtils.velocity = Vector2.zero;

				if (enemyHealth != null && Time.time > nextFire) {
					nextFire = Time.time + atkRate;
					enemyHealth.applyDamage (atkDmg);
				}
			} else if(!isLoopingPath() && isAtEndOfPath ()) {
				currentPath = null;
			} else {
				moveHero ();
			}
		}
		// If we have no path to the player then stand still
		else {
			steeringUtils.velocity = Vector2.zero;
		}

		touching.Clear ();
	}

	private void findPathToHero() {
		int[] end = map.worldToMapPoint(target.transform.position);
		
		if (currentPath == null || lastEndPos == null || lastEndPos [0] != end [0] || lastEndPos [1] != end [1]) { 
			currentPath = AStar.findPath (map, transform.position, target.transform.position, target);
			
			lastEndPos = end;
		}
	}

	bool isLoopingPath() {
		return (currentPath == patrolPath);
	}

	bool isAtEndOfPath () {
		return Vector3.Distance (currentPath.endNode, transform.position) < followPath.stopRadius;
	}

	void moveHero ()
	{
		Vector2 followAccel = followPath.getSteering (currentPath, isLoopingPath());
		Vector2 sepAccel = steeringUtils.separation (touching);
		
		//if (teamId == TEAM_1) {
			Vector3 foo = sepAccel;
			foo.Normalize();
			foo *= 3;
			Debug.DrawLine(transform.position, transform.position + foo, Color.cyan);
		//}
		
		steeringUtils.steer (followAccel + sepAccel);
		steeringUtils.lookWhereYoureGoing ();
		//currentPath.draw ();
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.layer != selectionBoxLayer) {
			touching[coll.transform] = coll;
		}
	}
}
