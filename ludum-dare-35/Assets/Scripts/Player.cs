using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

[RequireComponent(typeof(MovementAIRigidbody))]
public class Player: Health {
    public Transform bar;
    public Vector3 barSize = new Vector3(1, 1, 1);

    public GameObject losePanel;

    public float speed = 5;

    public float turnSpeed = 120f;

    private MovementAIRigidbody rb;

    private float horAxis = 0f;
    private float vertAxis = 0f;
    private float sideStepDir = 0f;

    public Transform bulletSpawnPoint;
    public Transform bulletPrefab;

    public float timeBetweenFire;
    private float nextFire;

    // Use this for initialization
    public override void Start()
    {
        rb = GetComponent<MovementAIRigidbody>();
        nextFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        horAxis = Input.GetAxisRaw("Horizontal");
        vertAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Q))
        {
            sideStepDir = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            sideStepDir = -1f;
        }
        else
        {
            sideStepDir = 0f;
        }

        if (Input.GetButton("Fire1") && nextFire < Time.time)
        {
            nextFire = Time.time + timeBetweenFire;

            Vector3 eulerAngles = bulletPrefab.eulerAngles;
            eulerAngles.y = transform.eulerAngles.y;
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(eulerAngles));
        }

        // Make the  bar scale to the current barProgress
        bar.localScale = new Vector3(percentHealth * barSize.x, barSize.y, barSize.z);
    }

    void FixedUpdate()
    {
        //rotateChar();
        moveChar();
    }

    private void moveChar()
    {
        Vector3 vel = new Vector3(horAxis, 0, vertAxis);

        if (vel.sqrMagnitude > 0.031)
        {
            rb.velocity = vel.normalized * speed;

            // Rotate the character
            float moveAngle = Mathf.Atan2(-vel.z, vel.x) * Mathf.Rad2Deg;
            if (moveAngle < 0)
            {
                moveAngle += 360;
            }

            //Debug.Log (moveAngle);
            rb.rotation = Quaternion.Euler(0, moveAngle, 0);
        } else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public override void outOfHealth()
    {
        losePanel.SetActive(true);

        if (bar != null)
        {
            Destroy(bar.gameObject);
        }

        Destroy(gameObject);
    }
}
