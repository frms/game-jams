using UnityEngine;
using System.Collections;

public class Enemy2AI : MonoBehaviour {
	public GameObject meleeObj;
	public float meleeArc = 180;
	public float meleeTime = 1;
	private float nextMelee = 0.0F;
	private bool attacking = false;

	private SteeringUtils steeringUtils;

	private Transform player;

	// Use this for initialization
	void Start () {
		steeringUtils = gameObject.GetComponent<SteeringUtils> ();

		player = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(player != null) {
			Vector2 sepAccel = steeringUtils.separation("Enemy");
			Vector2 arriveAccel = steeringUtils.arrive (player.position);

			if(sepAccel != Vector2.zero) {
				steeringUtils.steer (sepAccel);
			} else if(arriveAccel != Vector2.zero) {
				steeringUtils.steer (arriveAccel);
			}

			attacking = (Vector3.Distance(transform.position, player.position) <= steeringUtils.targetRadius);
		}
		// Else the player is dead so stop attacking and stop moving
		else {
			attacking = false;
			rigidbody2D.velocity = Vector2.zero;
		}
		
		updateMeleeAttack();
		
		steeringUtils.lookWhereYoureGoing ();
	}

	private void updateMeleeAttack() {
		// If the enemy should be attacking and is not already in a melee attack then start attacking
		if( attacking && Time.time > nextMelee) {
			nextMelee = Time.time + meleeTime;
		}
		
		// If we are still melee attacking then animate it
		if (nextMelee - Time.time >= 0) {
			float percent = Mathf.SmoothStep(0, 1, (1 - (nextMelee - Time.time) / meleeTime));
			float angle = meleeArc * percent;
			angle -= meleeArc/2f;

			Vector2 position = new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad));
			meleeObj.transform.localPosition = position*0.5f;
			meleeObj.transform.localRotation = Quaternion.Euler(0, 0, angle);
			
			meleeObj.SetActive(true);
		}
		// Else hide the attack image
		else {
			meleeObj.SetActive(false);
		}
	}
}
