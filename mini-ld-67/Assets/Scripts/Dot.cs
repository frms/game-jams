﻿using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour {
    public float[] speed;

    private Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();

        rb.velocity = GameManager.randomDir() * Random.Range(speed[0], speed[1]);
	}
}
