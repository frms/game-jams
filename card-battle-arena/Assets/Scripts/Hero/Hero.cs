using UnityEngine;
using System.Collections;

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
	private Rigidbody2D rb;

	// Use this for initialization
	public override void Start () {
		base.Start();

		map = GameObject.Find ("TileMap").GetComponent<MapBuilder> ().map;

		steeringUtils = GetComponent<SteeringUtils> ();
		followPath = GetComponent<FollowPath> ();
		rb = GetComponent<Rigidbody2D> ();
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
			followPathAndAtk (true);
		} else {
			followPathAndAtk (false);
		}

		// Control highlight
		highlight.SetActive (playerControlled || mouseIsOver || inSelectionBox);
	}

	private void playerControlledUpdate() {
		// Right Click
		if (Input.GetMouseButtonDown (1)) {
			target = castRay();

			if(target == null) {
				int[] start = map.worldToMapPoint(transform.position);
				Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				lastEndPos = map.worldToMapPoint(worldPoint);
				
				target = map.objs [lastEndPos [0], lastEndPos [1]];

				currentPath = AStar.findPath (map, start, lastEndPos, (Base) target);
			}
			
			if(target != null && target.teamId != TeamMember.TEAM_1) {
				enemyHealth = target.GetComponent<HealthBar>();
			} else {
				enemyHealth = null;
			}
		}

		followPathAndAtk (false);
	}

	private Hero castRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		if (hit) {
			return hit.transform.GetComponent<Hero>();
		}

		return null;
	}

	private void findPathToHero() {
		int[] end = map.worldToMapPoint(target.transform.position);
		
		if (currentPath == null || lastEndPos == null || lastEndPos [0] != end [0] || lastEndPos [1] != end [1]) { 
			
			int[] start = map.worldToMapPoint(transform.position);
			
			currentPath = AStar.findPath (map, start, end, null);

			//Remove the last path node so we go right up to but no on top of the target hero
			if(currentPath != null && currentPath.Length > 1) {
				currentPath.removeNode(currentPath.Length - 1);
			}

			lastEndPos = end;
		}
	}

	void followPathAndAtk (bool pathLoop)
	{
		if (target != null && target is Hero) {
			findPathToHero();
		}

		// Follow path and atk target if its an enemy
		if (currentPath != null) {
			if (target != null && isAtEndOfPath ()) {
				//Look at the target and stop moving
				steeringUtils.lookAtDirection (target.transform.position - transform.position);
				rb.velocity = Vector2.zero;
				if (enemyHealth != null && Time.time > nextFire) {
					nextFire = Time.time + atkRate;
					enemyHealth.applyDamage (atkDmg);
				}
			}
			else {
				moveHero (pathLoop);
			}
		}
		// If we have no path to the player then stand still
		else {
			rb.velocity = Vector2.zero;
		}
	}

	bool isAtEndOfPath ()
	{
		return Vector3.Distance (currentPath.endNode, transform.position) < followPath.stopRadius;
	}

	void moveHero (bool pathLoop)
	{
		Vector2 accel = followPath.getSteering (currentPath, pathLoop);
		steeringUtils.steer (accel);
		steeringUtils.lookWhereYoureGoing ();
		currentPath.draw ();
	}
}
