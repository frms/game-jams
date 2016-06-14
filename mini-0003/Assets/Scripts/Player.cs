using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed;
    public float jump;

    private bool onGround = false;

    private Rigidbody2D rb;

    private float radius;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();

        CircleCollider2D col = GetComponent<CircleCollider2D>();
        radius = Mathf.Max(transform.localScale.x, transform.localScale.y) * col.radius;
    }

    private bool upDown = false;

	// Update is called once per frame
    void Update ()
    {
        float yAxis = Input.GetAxisRaw("Vertical");

        if(yAxis > 0 && upDown == false)
        {
            if(onGround)
            {
                Vector2 vel = rb.velocity;
                vel.y += jump;
                rb.velocity = vel;
            }

            upDown = true;
        }

        if(yAxis <= 0)
        {
            upDown = false;
        }
    }

	void FixedUpdate ()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        Vector2 vel = rb.velocity;
        vel.x = xAxis * speed;
        rb.velocity = vel;

        checkGround();
	}

    private void checkGround()
    {
        Vector2 origin = rb.position + (2 * Physics2D.minPenetrationForPenalty * Vector2.up);
        float dist = 3 * Physics2D.minPenetrationForPenalty;
        RaycastHit2D hit = Physics2D.CircleCast(origin, radius, Vector2.down, dist);
        onGround = (hit.collider != null);

        Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red, 0f, false);
        Debug.Log((hit.collider != null) ? hit.collider.name : "none");
    }
}
