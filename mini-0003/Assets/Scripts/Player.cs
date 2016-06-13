using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        Vector2 vel = Vector2.zero;
        vel.x = xAxis * speed;
        rb.velocity = vel;

        Debug.Log(xAxis + " " + vel.ToString("F4"));
	}
}
