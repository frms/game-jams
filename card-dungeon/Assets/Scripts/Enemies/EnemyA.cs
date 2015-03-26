using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyA : MonoBehaviour, IHealth {
	public float health = 200f;
	public float maxHealth = 200f;

	public float atkDist = 2;

	private MapData map;
	private SteeringUtils steeringUtils;
	private FollowPath followPath;
	private Rigidbody2D rb;
	private Transform player;
	private EnemyLaser laser;
	private SpriteRenderer sr;
	
	// Use this for initialization
	void Start () {
		map = GameObject.Find ("Map").GetComponent<TileMap> ().map;
		steeringUtils = GetComponent<SteeringUtils> ();
		followPath = GetComponent<FollowPath> ();
		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("Player").transform;
		laser = GetComponentInChildren<EnemyLaser> ();
		sr = GetComponent<SpriteRenderer> ();
	}
	
	private LinePath currentPath = null;

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
			
			laser.fire(hit);
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

			currentPath = AStar.findPath (map, start, end);

			lastPlayerPos = end;
		}
	}
	
	public void takeDamage(float dmg) {
		health -= dmg;
		sr.color = new Color (1, 1, 1, 0.5f);

		CancelInvoke ();
		Invoke ("resetColor", 0.25f);
	}

	private void resetColor() {
		sr.color = Color.white;
	}

}
