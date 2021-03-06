﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public float speed = 5;

	public GameObject meleeImage;
	public float meleeArc = 180;
	public float meleeTime = 1;
	private float nextMelee = 0.0F;

	public int numOfElementA = 0;
	public int numOfElementB = 0;
	public int numOfElementC = 0;

	public Ability secondaryAbility = Ability.None;

	public enum Ability {None, Arrow, Dash, Bomb};

	public GameObject projectile;
	public Transform spawnPoint;
	public float maxArrowDistance = 10;
	public float arrowSpeed = 10;
	public float arrowDamage = 10;

	public float dashDistance = 10;
	public float dashSpeed = 15;
	private bool dashing = false;
	private Vector3 dashStartPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(!dashing) {
			updateMoveCharacter();
		}

		updateMeleeAttack ();

		// Trigger secondary action
		if (Input.GetButtonDown ("Fire2")) {

			switch (secondaryAbility) {
				case Ability.Arrow:
					fireArrow(maxArrowDistance);
					break;
				case Ability.Dash:
					startDashing();
					break;
			}
		}

		if(dashing && Vector3.Distance(transform.position, dashStartPoint) >= dashDistance) {
			stopDashing();
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		// If we're dashing and we collide with anything then stop dashing
		if(dashing) {
			stopDashing();
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		// If we're dashing and we collide with anything then stop dashing
		if(dashing) {
			stopDashing();
		}
	}

	private void updateMoveCharacter() {
		Vector2 stickDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		
		//stickDirection = stickDirection.normalized;
		
		if(stickDirection.sqrMagnitude > 0.031) {
			
			// Rotate the character
			float moveAngle = Mathf.Atan2(stickDirection.y, stickDirection.x)*Mathf.Rad2Deg;
			if(moveAngle < 0) {
				moveAngle += 360;
			}
			
			//Debug.Log (moveAngle);

			transform.rotation = Quaternion.Euler(0, 0, moveAngle);
			rigidbody2D.velocity = transform.right * speed;
		} else {
			rigidbody2D.velocity = Vector2.zero;
		}
	}

	private void updateMeleeAttack() {
		// If melee button is hit and not already in a melee attack then start attacking
		if(Input.GetButtonDown("Fire1") && Time.time > nextMelee) {
			nextMelee = Time.time + meleeTime;
		}

		// If we are still melee attacking then animate it
		if (nextMelee - Time.time >= 0) {
			float angle = meleeArc * (1 - (nextMelee - Time.time) / meleeTime);
			angle -= meleeArc/2f;
			
			Vector2 position = new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad));
			meleeImage.transform.localPosition = position*0.6f;
			meleeImage.transform.localRotation = Quaternion.Euler(0, 0, angle);
			
			meleeImage.SetActive(true);
		}
		// Else hide the attack image
		else {
			meleeImage.SetActive(false);
		}
	}

	private void fireArrow(float distance) {
		GameObject clone = Instantiate (projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
		
		ArrowProperties props = new ArrowProperties ();
		props.maxDistance = distance;
		props.damage = arrowDamage;
		clone.SendMessage ("setUp", props);

		clone.rigidbody2D.velocity = transform.right * arrowSpeed;
	}

	private void startDashing() {
		dashing = true;

		dashStartPoint = transform.position;
		rigidbody2D.velocity = transform.right * dashSpeed;
	}

	private void stopDashing() {
		dashing = false;
		rigidbody2D.velocity = Vector2.zero;
	}
	
	public void gainElementA() {
		numOfElementA++;
		
		secondaryAbility = Ability.Arrow;
	}
	
	public void gainElementB() {
		numOfElementB++;
		
		secondaryAbility = Ability.Dash;
	}
	
	public void gainElementC() {
		numOfElementC++;
		
		secondaryAbility = Ability.Bomb;
	}

}
