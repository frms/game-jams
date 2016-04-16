using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = -transform.up * speed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
