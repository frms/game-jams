using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float thrust;
    public float maxSpeed;
    public float turnSpeed;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        float horAxis = Input.GetAxisRaw("Horizontal");
        rb.angularVelocity = new Vector3(0, horAxis * turnSpeed * Mathf.Deg2Rad, 0);

        float vertAxis = Input.GetAxisRaw("Vertical");
        rb.AddForce(transform.right * vertAxis * thrust, ForceMode.Acceleration);

        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
