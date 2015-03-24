using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyA : MonoBehaviour {
	public float atkDist = 2;

	private MapData map;
	private SteeringUtils steeringUtils;
	private FollowPath followPath;
	private Rigidbody2D rb;
	private Transform player;
	private EnemyLaser laser;
	
	// Use this for initialization
	void Start () {
		map = GameObject.Find ("Map").GetComponent<TileMap> ().map;
		steeringUtils = GetComponent<SteeringUtils> ();
		followPath = GetComponent<FollowPath> ();
		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("Player").transform;
		laser = GetComponentInChildren<EnemyLaser> ();
	}
	
	private LinePath currentPath = null;
	
	// Update is called once per frame
	void FixedUpdate () {
		float dist = Vector3.Distance (transform.position, player.position);
		Vector3 direction = player.position - transform.position;
		RaycastHit2D hit = Physics2D.Raycast (transform.position, direction);

		if (dist > atkDist || hit.collider == null || hit.collider.tag != "Player") {
			findPathToPlayer ();

			Vector2 accel = followPath.getSteering (currentPath);
			steeringUtils.steer (accel);
			steeringUtils.lookWhereYoureGoing ();

			laser.stop();
		} else {
			rb.velocity = Vector2.zero;
			steeringUtils.lookAtDirection(direction);
			
			laser.fire(hit.point);
		}
	}

	private int[] lastPlayerPos;

	private void findPathToPlayer() {
		int[] end = new int[2];
		end [0] = Mathf.FloorToInt (player.position.x);
		end [1] = Mathf.FloorToInt (player.position.y);

		if (currentPath == null || lastPlayerPos == null || lastPlayerPos [0] != end [0] || lastPlayerPos [1] != end [1]) { 

			int[] start = new int[2];
			start [0] = Mathf.FloorToInt (transform.position.x);
			start [1] = Mathf.FloorToInt (transform.position.y);

			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();

			currentPath = AStar.findPath (map, start, end);

			sw.Stop();

			Debug.Log (sw.Elapsed.TotalMilliseconds);

			//currentPath.draw();

			lastPlayerPos = end;
		}
	}

}
