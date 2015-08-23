using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy1 : Mover {

	public bool isLooping;

	private Renderer rend;
	private Color initialColor;

	private static Color r = new Color( 1f, 0.341f, 0.133f);
	private static Color w = new Color( 0.933f, 0.933f, 0.933f);

	internal Transform playerTrans;

	public override void Start() {
		base.Start();

		rend = GetComponent<Renderer>();
		initialColor = rend.material.color;

		nextWander = Time.time + Random.Range(jumpStartWanderRate[0], jumpStartWanderRate[1]);

		playerTrans = GameObject.Find("Player").transform;
	}

	internal bool isMouseOver = false;
	private Color rememberColor;

	public virtual void OnMouseEnter() {
		rememberColor = rend.material.color;
		rend.material.color = r;
		isMouseOver = true;
	}
	
	public virtual void OnMouseExit() {
		rend.material.color = rememberColor;
		isMouseOver = false;
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

	public float decayTime = 3f;

	public void IAmDead() {
		timeToDisappear = Time.time + decayTime;

		Map.enemiesRemaining--;
	}

	public bool IAmBeingEaten = false;
	public float eatenSpeed = 5f;

	public void checkForDeath() {

		if(IAmBeingEaten) {
			float step = eatenSpeed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, playerTrans.position, step);

			float dist = Vector3.Distance(transform.position, playerTrans.position);
			if( dist < 1f ) {
				transform.localScale = new Vector3(dist, dist, dist);
			}

			if(transform.position == playerTrans.position) {
				givePlayerLove();
				Destroy(gameObject);
			}

			return;
		}

		if(timeToDisappear != Mathf.Infinity) {
			Vector2 accel = steeringUtils.arrive (Map.map.mapToWorldPoint (reservedPos [0], reservedPos [1]));
			
			steeringUtils.steer (accel);
			steeringUtils.lookWhereYoureGoing ();

			if(!isMouseOver) {
				float t = (timeToDisappear - Time.time) / decayTime;
				rend.material.color = Color.Lerp(w, initialColor, t);
			}
		}

		if(!IAmBeingEaten && timeToDisappear < Time.time) {
			Destroy(gameObject);
		}
	}

	public virtual void givePlayerLove() {
		HealthBar hb = playerTrans.GetComponent<HealthBar>();
		hb.applyDamage(-15f);
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
