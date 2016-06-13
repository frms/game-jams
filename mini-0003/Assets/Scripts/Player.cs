using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed;
    public float jump;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
    void Update ()
    {
        if(Input.GetButtonDown("Jump"))
        {
            Vector2 vel = rb.velocity;
            vel.y += jump;
            rb.velocity = vel;
        }
    }

	void FixedUpdate ()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        Vector2 vel = rb.velocity;
        vel.x = xAxis * speed;
        rb.velocity = vel;
	}
}
