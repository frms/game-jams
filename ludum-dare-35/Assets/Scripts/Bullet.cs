﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed;
    public float dmg;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = -transform.up * speed;
	}
}