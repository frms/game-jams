using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy1 : Mover {

	public bool isLooping;

	private Renderer rend;
	private Color initialColor;

	private static Color r = new Color( 1f, 0.341f, 0.133f);

	internal Transform playerTrans;

	public override void Start() {
		base.Start();

		rend = GetComponent<Renderer>();
		initialColor = rend.material.color;

		nextWander = Time.time + Random.Range(jumpStartWanderRate[0], jumpStartWanderRate[1]);

		playerTrans = GameObject.Find("Player").transform;
	}
	
	public virtual void OnMouseEnter() {
		rend.material.color = r;
	}
	
	public virtual void OnMouseExit() {
		rend.material.color = initialColor;
	}

	// Update is called once per frame
	public virtual void Update () {
		checkForDeath();
		if(timeToDisappear != Mathf.Infinity || IAmBeingEaten) {
			return;
		}

		if(isLooping && currentPath != null && isAtEndOfPath()) {
			currentPath = AStar.findPath(Map.map, currentPath.endNode, currentPath[0], null, 0);
		}

		moveUnit ();
	}

	internal float timeToDisappear = Mathf.Infinity;

	public bool isDead() {
		return timeToDisappear != Mathf.Infinity;
	}

	public void IAmDead() {
		timeToDisappear = Time.time + 3f;

		Map.enemiesRemaining--;
	}

	public bool IAmBeingEaten = false;
	public float eatenSpeed = 3f;

	public void checkForDeath() {

		if(IAmBeingEaten) {
			float step = eatenSpeed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, playerTrans.position, step);

			float dist = Vector3.Distance(transform.position, playerTrans.position);
			if( dist < 1f ) {
				transform.localScale = new Vector3(dist, dist, dist);
			}

			if(transform.position == playerTrans.position) {
				Destroy(gameObject);
			}

			return;
		}

		if(timeToDisappear != Mathf.Infinity) {
			Vector2 accel = steeringUtils.arrive (Map.map.mapToWorldPoint (reservedPos [0], reservedPos [1]));
			
			steeringUtils.steer (accel);
			steeringUtils.lookWhereYoureGoing ();
		}

		if(!IAmBeingEaten && timeToDisappear < Time.time) {
			Destroy(gameObject);
		}
	}

	public bool isAtEndOfPath () {
		return Vector3.Distance (currentPath.endNode, transform.position) < followPath.stopRadius;
	}

	public int wanderBoxSize = 3;
	public float nextWander = 0;

	public float[] wanderRate = new [] {4f, 6f};
	public float[] jumpStartWanderRate = new [] {0f, 1.5f};

	public void tryToWander() {
		if (Time.time > nextWander) {
			nextWander = Time.time + Random.Range(wanderRate[0], wanderRate[1]);
			
			List<int[]> openSpots = new List<int[]>();
			
			int xMin = Mathf.Max(reservedPos[0] - wanderBoxSize, 0);
			int xMax = Mathf.Min(reservedPos[0] + wanderBoxSize, Map.map.width - 1);
			
			int yMin = Mathf.Max(reservedPos[1] - wanderBoxSize, 0);
			int yMax = Mathf.Min(reservedPos[1] + wanderBoxSize, Map.map.height - 1);
			
			for(int x = xMin; x <= xMax; x++) {
				for(int y = yMin; y <= yMax; y++) {
					if(Map.map.isWalkable(x, y, null, false)) {
						openSpots.Add(new [] {x, y});
					}
				}
			}
			
			int[] randomSpot = openSpots[Random.Range(0, openSpots.Count)];
			
			findPath(Map.map.mapToWorldPoint(randomSpot[0], randomSpot[1]));
		}
	}
}
