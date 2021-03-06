using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Hero : TeamMember {
	public float atkRate = 0.5F;
	public float atkDmg = 5f;

	public Transform bullet;

	public float repathTimeout = 1f;
	public float arriveAfterPathTimeout = 1f;

	[System.NonSerialized]
	public bool playerControlled;

	[System.NonSerialized]
	public bool inSelectionBox;

	[System.NonSerialized]
	public LinePath patrolPath;

	private MapData map;

	private SteeringUtils steeringUtils;
	private FollowPath followPath;
	private Rigidbody2D rb;

	private HashSet<Transform>  touching = new HashSet<Transform> ();

	private NearSensor collAvoidSensor;
	private NearSensor atkRange;

	// Use this for initialization
	public override void Start () {
		base.Start();

		map = GameObject.Find ("TileMap").GetComponent<MapBuilder> ().map;

		steeringUtils = GetComponent<SteeringUtils> ();
		steeringUtils.collAvoidRadius = GetComponent<CircleCollider2D> ().radius;
		followPath = GetComponent<FollowPath> ();
		rb = GetComponent<Rigidbody2D> ();

		collAvoidSensor = transform.Find ("CollAvoidSensor").GetComponent<NearSensor> ();
		atkRange = transform.Find ("AtkRange").GetComponent<NearSensor> ();
	}

	private int[] lastEndPos;

	private LinePath currentPath;
	private float farthestPathParam;
	private float farthestPathTime;
	private Vector3 arriveTarget;
	private float arriveStartTime = float.NegativeInfinity;

	private TeamMember target;
	private HealthBar enemyHealth;
	private float nextFire = 0.0F;


	// Update is called once per frame
	void Update () {
		if (playerControlled) {
			playerControlledUpdate ();
		} else if (patrolPath != null) {
			currentPath = patrolPath;
		}
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

				findPath (map, transform.position, worldPoint, target);
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
		
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity, GameManager.heroMask);
		if (hit) {
			return hit.transform.GetComponent<Hero>();
		}

		return null;
	}

	private void findPath(MapData graph, Vector3 startPos, Vector3 endPos, TeamMember target) {
		currentPath = AStar.findPath (graph, startPos, endPos, target);
		farthestPathParam = float.NegativeInfinity;
		farthestPathTime = Time.time;

		arriveStartTime = float.NegativeInfinity;
	}

	public override void LateUpdate () {
		// Control highlight
		highlight.SetActive (playerControlled || mouseIsOver || inSelectionBox);
	}

	void FixedUpdate () {
		rb.constraints = RigidbodyConstraints2D.FreezeAll;

		if (target != null && target is Hero) {
			findPathToHero();
		}

		// Follow path and atk target if its an enemy
		if (currentPath != null) {
			if (target != null && atkRange.targets.Contains (target.transform)) {
				//Look at the target and stop moving
				steeringUtils.lookAtDirection (target.transform.position - transform.position);
				rb.velocity = Vector2.zero;

				if (enemyHealth != null && Time.time > nextFire) {
					nextFire = Time.time + atkRate;
					Transform clone = Instantiate (bullet, transform.position, Quaternion.identity) as Transform;
					clone.GetComponent<Bullet> ().setUp (enemyHealth, atkDmg);
				}
			} else if (!isLoopingPath () && (isAtEndOfPath () || (Time.time - farthestPathTime) > repathTimeout)) {
				if(!isAtEndOfPath ()) {
					setArriveTarget ();
				}

				currentPath = null;
			} else {
				moveAlongPath ();
			}
		} else if (arriveStartTime + arriveAfterPathTimeout > Time.time) {
			moveToTarget ();
		}
		// If we have no path to the player then stand still
		else {
			rb.velocity = Vector2.zero;
		}
	}

	private void findPathToHero() {
		int[] end = map.worldToMapPoint(target.transform.position);
		
		if (currentPath == null || lastEndPos == null || lastEndPos [0] != end [0] || lastEndPos [1] != end [1]) { 
			findPath (map, transform.position, target.transform.position, target);
			
			lastEndPos = end;
		}
	}

	bool isLoopingPath() {
		return (currentPath == patrolPath);
	}

	bool isAtEndOfPath () {
		return Vector3.Distance (currentPath.endNode, transform.position) < followPath.stopRadius;
	}

	void setArriveTarget ()
	{
		if (currentPath.Length > 1) {
			arriveTarget = currentPath.getPosition (farthestPathParam);
		}
		else {
			arriveTarget = currentPath [0];
		}

		arriveStartTime = Time.time;
	}

	void moveToTarget ()
	{
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

		Vector2 arriveAccel = steeringUtils.arrive (arriveTarget);

		steeringUtils.steer (arriveAccel);
		steeringUtils.lookWhereYoureGoing ();
	}

	void moveAlongPath ()
	{
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

		updatePathParam ();
		
		// Clean up any destroyed targets
		collAvoidSensor.targets.RemoveWhere(t => t == null);

		Vector2 followAccel = followPath.getSteering (currentPath, isLoopingPath());

		Transform ignoreUnit = (target != null) ? target.transform : null;
		Vector2 collAvoidAccel = steeringUtils.collisionAvoidance (collAvoidSensor.targets, ignoreUnit);
		
		steeringUtils.steer (followAccel + 2*collAvoidAccel);
		steeringUtils.lookWhereYoureGoing ();

		// Debug drawing
		Vector3 foo = collAvoidAccel.normalized * 3;
		Debug.DrawLine(transform.position, transform.position + foo, Color.cyan);

		currentPath.draw ();
	}

	void updatePathParam ()
	{
		float param;

		if (currentPath.Length > 1) {
			param = currentPath.getParam (transform.position);
		}
		else {
			param = Vector2.Distance (transform.position, currentPath [0]);
		}

		if (param > farthestPathParam) {
			farthestPathParam = param;
			farthestPathTime = Time.time;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.layer != GameManager.selectionBoxLayer) {
			touching.Add(coll.transform);
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.layer != GameManager.selectionBoxLayer) {
			touching.Remove(coll.transform);
		}
	}
}
