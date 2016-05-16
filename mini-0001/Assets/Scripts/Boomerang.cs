using UnityEngine;
using System.Collections;

public class Boomerang : MonoBehaviour
{

    public float dist;
    public float speed;
    public float dmg;

    private Transform player;
    private Vector2 targetPos;
    private bool hasReachedTarget;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").transform;

        if (player.localScale.x < 0)
        {
            targetPos = player.position + Vector3.left * dist;
        }
        else
        {
            targetPos = player.position + Vector3.right * dist;
        }

        hasReachedTarget = false;

        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }

        float step = speed * Time.deltaTime;

        if (!hasReachedTarget)
        {
            if (rb.position != targetPos)
            {
                rb.MovePosition(Vector2.MoveTowards(rb.position, targetPos, step));
            }
            else
            {
                hasReachedTarget = true;
            }
        }
        else
        {
            if (Vector2.Distance(rb.position, player.position) > 0.5f)
            {
                rb.MovePosition(Vector2.MoveTowards(rb.position, player.position, step));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Health h = other.GetComponent<Health>();
            h.applyDamage(transform, dmg);
        }
    }
}