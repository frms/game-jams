using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyA : MonoBehaviour {
	private MapData map;
	private SteeringUtils steeringUtils;
	private FollowPath followPath;
	private Rigidbody2D rb;
	private Transform player;
	
	// Use this for initialization
	void Start () {
		map = GameObject.Find ("Map").GetComponent<TileMap> ().map;
		steeringUtils = GetComponent<SteeringUtils> ();
		followPath = GetComponent<FollowPath> ();
		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("Player").transform;
	}

	private bool followPlayer = false;
	private LinePath currentPath = null;
	
	// Update is called once per frame
	void FixedUpdate () {
		// A Button
		if (Input.GetButtonDown ("Fire1")) {
			followPlayer = false;
			currentPath = null;
		}
		// B Button
		else if (Input.GetButtonDown ("Fire2")) {
			followPlayer = true;
		}

		if (followPlayer) {
			findPathToPlayer();
		}

		if (currentPath != null) {
			Vector2 accel = followPath.getSteering (currentPath);
			steeringUtils.steer (accel);
		} else {
			rb.velocity = Vector2.zero;
		}

		steeringUtils.lookWhereYoureGoing ();
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

			currentPath = AStar.findPath (map, start, end);

			lastPlayerPos = end;
		}
	}

}
