using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float dist;
    public Vector2 hitVel;

    private Vector2 startPos;
    private Rigidbody2D rb;

	// Use this for initialization
	void Start ()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Vector2.Distance(startPos, transform.position) >= dist)
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name != "Player")
        {
            if (other.tag == "Movable")
            {
                hitVel.x = Mathf.Abs(hitVel.x);
                if (rb.velocity.x < 0)
                {
                    hitVel.x *= -1;
                }

                other.GetComponent<Rigidbody2D>().velocity += hitVel;
            }

            Destroy(gameObject);
        }
    }
}
