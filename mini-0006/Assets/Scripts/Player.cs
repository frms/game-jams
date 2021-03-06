﻿using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 8.413461538f;

    public GameObject passThroughPrefab;
    public float passThroughSpeed = 8.413461538f;
    public float passThroughDist = 4.5f;

    private GameObject passThroughPath;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private float radius;
    private float overlapRadius;

    // Use this for initialization
    void Start()
    {
        passThroughPath = Instantiate(passThroughPrefab);
        passThroughPath.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        radius = Mathf.Max(rb.transform.localScale.x, rb.transform.localScale.y) * col.radius;

        /* I create an overlap radius that is slightly smaller than the actual radius.
         * I do this because my tests seem to show that Physics2D.circleCast() can 
         * return results where the circle is actually overlapping with a collider
         * by <= Physics2D.minPenetrationForPenalty. And my tests also seem to 
         * suggest that Physics2D.OverlapCircle() will consider a circle overlaping
         * even if it is not overlapping but it is less than Physics2D.minPenetrationForPenalty
         * away from a collider (which matches up what you see with most colliders coming
         * to rest slightly less than Physics2D.minPenetrationForPenalty away from other
         * colliders in the scene). So I must remove Physics2D.minPenetrationForPenalty
         * twice from the radius to make sure our overlap test does not count any colliders
         * that the circle is right up against. */
        overlapRadius = radius - (2f * Physics2D.minPenetrationForPenalty);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            passThrough(Vector2.right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            passThrough(Vector2.left);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            passThrough(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            passThrough(Vector2.down);
        }
    }

    private Vector2 passThroughDest;

    private void passThrough(Vector2 dir)
    {
        passThroughDest = rb.position;

        Vector2 endPos = rb.position + dir * passThroughDist;

        if (Physics2D.OverlapCircle(endPos, overlapRadius) == null)
        {
            passThroughDest = endPos;
        }
        else
        {
            RaycastHit2D[] hits = PhysicsHelper.circleCastAll(rb.position, radius, dir, passThroughDist);

            for (int i = hits.Length - 1; i >= 0; i--)
            {
                if (Physics2D.OverlapCircle(hits[i].centroid, overlapRadius) == null)
                {
                    passThroughDest = hits[i].centroid;
                    break;
                }
            }
        }

        passThroughPath.SetActive(true);

        Debug.DrawLine(rb.position, endPos, Color.white, 1.0f, false);
        Debug.DrawLine(rb.position, passThroughDest, Color.green, 1.0f, false);
    }

    void FixedUpdate()
    {
        if(passThroughPath.activeSelf)
        {
            col.isTrigger = true;

            Vector2 disp = passThroughDest - rb.position;

            if(disp.magnitude > passThroughSpeed * Time.deltaTime)
            {
                rb.velocity = disp.normalized * passThroughSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;
                rb.position = passThroughDest;
                passThroughPath.SetActive(false);
            }
        }
        else
        {
            col.isTrigger = false;

            Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            dir.Normalize();
            rb.velocity = dir * speed;

            if (dir.magnitude > 0f)
            {
                rb.rotation = Mathf.Atan2(dir[1], dir[0]) * Mathf.Rad2Deg;
            }
        }
    }

    void LateUpdate()
    {
        if (passThroughPath.activeSelf)
        {
            Vector3 disp = (Vector3) passThroughDest - transform.position;
            float dist = disp.magnitude;
            float zRot = Mathf.Atan2(disp.y, disp.x) * Mathf.Rad2Deg;

            passThroughPath.transform.position = transform.position + disp / 2f;
            passThroughPath.transform.localScale = new Vector3(dist, radius, 1f);
            passThroughPath.transform.rotation = Quaternion.Euler(0, 0, zRot);
        }
    }
}
