using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy2 : Enemy1 {
	public float seekTargetDist = 10f;

	private Mover player;
	private HealthBar playerHealth;

	// Use this for initialization
	public override void Start () {
		base.Start();

		GameObject playerGO = GameObject.Find("Player");
		player = playerGO.GetComponent<Mover>();
		playerHealth = player.GetComponent<HealthBar>();
	}

	private bool wasFollowingPlayer = false;

	// Update is called once per frame
	public override void Update () {
		if(player != null && Vector2.Distance(player.transform.position, transform.position) < seekTargetDist) {
			target = player;
			enemyHealth = playerHealth;

			wasFollowingPlayer = true;
		} else {
			target = null;
			enemyHealth = null;

			if(wasFollowingPlayer) {
				nextWander = Time.time + Random.Range(jumpStartWanderRate[0], jumpStartWanderRate[1]);
			}

			wasFollowingPlayer = false;

			tryToWander();
		}

		moveUnit ();

		tryToAttack();
	}
}
