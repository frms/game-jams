using UnityEngine;
using System.Collections;

public class Hero : TeamMember {
	public float atkRate = 0.5F;
	public float atkDmg = 5f;

	[System.NonSerialized]
	public bool playerControlled;

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
	private TeamMember target;
	private HealthBar enemyHealth;
	private float nextFire = 0.0F;

	// Update is called once per frame
	void Update () {
		if (playerControlled) {
			playerControlledUpdate();
		}
	}

	private void playerControlledUpdate() {
		// Right Click
		if (Input.GetMouseButtonDown (1)) {
			target = castRay();

			if(target == null) {
				int[] start = map.worldToMapPoint(transform.position);
				Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				int[] end = map.worldToMapPoint(worldPoint);
				
				target = map.objs [end [0], end [1]];

				currentPath = AStar.findPath (map, start, end, (Base) target);
			}
			
			if(target != null && target.team != TeamMember.TEAM_1) {
				enemyHealth = target.GetComponent<HealthBar>();
			} else {
				enemyHealth = null;
			}
		}

		if (target != null && target is Hero) {
			findPathToHero();
		}

		// Follow path and atk target if its an enemy
		if(currentPath != null) {
			if(target != null && isAtEndOfPath ()) {
				//Look at the target and stop moving
				steeringUtils.lookAtDirection(target.transform.position - transform.position);
				rb.velocity = Vector2.zero;
				
				if(enemyHealth != null && Time.time > nextFire) {
					nextFire = Time.time + atkRate;
					enemyHealth.applyDamage(atkDmg);
				}
			} else {
				moveHero ();
			}
		}
		// If we have no path to the player then stand still
		else {
			rb.velocity = Vector2.zero;
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

	private int[] lastHeroPos;
	
	private void findPathToHero() {
		int[] end = map.worldToMapPoint(target.transform.position);
		
		if (currentPath == null || lastHeroPos == null || lastHeroPos [0] != end [0] || lastHeroPos [1] != end [1]) { 
			
			int[] start = map.worldToMapPoint(transform.position);
			
			currentPath = AStar.findPath (map, start, end, null);
			
			lastHeroPos = end;
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
