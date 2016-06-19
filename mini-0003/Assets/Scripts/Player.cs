using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public float jump;
    public float groundCheckDist;
    public Transform bulletPrefab;
    public float bulletSpeed;
    private Transform lastBullet = null;

    private float facing = 1;
    private Transform eye;
    private bool onGround = false;

    private Rigidbody2D rb;

    private float radius;

	// Use this for initialization
	void Start ()
    {
        eye = transform.FindChild("Eye");

        rb = GetComponent<Rigidbody2D>();

        CircleCollider2D col = GetComponent<CircleCollider2D>();
        radius = Mathf.Max(transform.localScale.x, transform.localScale.y) * col.radius;
    }

	// Update is called once per frame
    void Update ()
    {
        if(rb.position.y < -5)
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene);
        }

        updateFacing();

        /* Using Jump button for atk (and up button for jump) */
        if (Input.GetButtonDown("Jump") && lastBullet == null)
        {
            lastBullet = Instantiate(bulletPrefab, rb.position, Quaternion.identity) as Transform;
            lastBullet.GetComponent<Rigidbody2D>().velocity = facing * bulletSpeed * Vector2.right;
        }

        tryToJump();
    }

    private void updateFacing()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        if (xAxis > 0)
        {
            facing = 1;
        }
        else if (xAxis < 0)
        {
            facing = -1;
        }

        Vector3 pos = eye.localPosition;
        pos.x = Mathf.Abs(pos.x) * facing;
        eye.localPosition = pos;
    }

    private bool upDown = false;

    private void tryToJump()
    {
        float yAxis = Input.GetAxisRaw("Vertical");

        if (yAxis > 0 && upDown == false)
        {
            if (onGround)
            {
                Vector2 vel = rb.velocity;
                vel.y += jump;
                rb.velocity = vel;
            }

            upDown = true;
        }

        if (yAxis <= 0)
        {
            upDown = false;
        }
    }

	void FixedUpdate ()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        Vector2 vel = rb.velocity;
        vel.x = xAxis * speed;

        RaycastHit2D hit;
        if (circleCast(vel.normalized, out hit, vel.magnitude * Time.fixedDeltaTime))
        {
            /* Move up to whatever we hit */
            rb.position += vel.normalized * Mathf.Max(0, hit.distance);

            /* Project our velocity against what we hit so we don't move it. Obviously 
             * we'd need to keep checking the projected velocity if we really care to
             * avoid moving stuff. */
            Vector2 slope;
            slope.x = -hit.normal.y;
            slope.y = hit.normal.x;
            vel = Vector3.Project(vel, slope);
        }

        rb.velocity = vel;

        Debug.DrawLine(rb.position, rb.position + rb.velocity.normalized, Color.red, 0f, false);

        checkGround();
	}

    private void checkGround()
    {
        RaycastHit2D hit;
        onGround = circleCast(Vector2.down, out hit, groundCheckDist);
    }

    private bool circleCast(Vector2 dir, out RaycastHit2D hit, float dist)
    {
        Vector2 origin = rb.position - Physics2D.minPenetrationForPenalty * dir;
        float maxDist = Physics2D.minPenetrationForPenalty + dist;
        
        hit = Physics2D.CircleCast(origin, radius, dir, maxDist);

        if(hit.collider != null)
        {
            /* I'm not sure why but it seems that I need to subtract the
             * Physics2D.minPenetrationForPenalty twice (instead of what should be once).
             * It seems like in Physics 2D the min penetration is more like a min
             * separation and overlap is never allowed. */
            hit.distance -= 2 * Physics2D.minPenetrationForPenalty;
            return true;
        }
        else
        {
            return false;
        }
    }
}
