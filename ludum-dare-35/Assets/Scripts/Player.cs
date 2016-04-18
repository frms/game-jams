using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public Transform bulletSpawnPoint;
    public Transform bulletPrefab;

    public float timeBetweenFire;
    private float nextFire;
    private Flash ff;

    public float bulletRandom;
    public float whiskers;

    private AudioSource shootClip;

    // Use this for initialization
    public override void Start()
    {
        rb = GetComponent<MovementAIRigidbody>();
        ff = GetComponentInChildren<Flash>();
        nextFire = Time.time;

        shootClip = transform.FindChild("ShootSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horAxis = Input.GetAxisRaw("Horizontal");
        vertAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Fire1") && nextFire < Time.time)
        {
            nextFire = Time.time + timeBetweenFire;

            //ff.flash();

            Vector3 eulerAngles = bulletPrefab.eulerAngles;

            eulerAngles.y = transform.eulerAngles.y - whiskers + Random.Range(-bulletRandom, bulletRandom);
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(eulerAngles));

            eulerAngles.y = transform.eulerAngles.y + Random.Range(-bulletRandom, bulletRandom);
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(eulerAngles));

            eulerAngles.y = transform.eulerAngles.y + whiskers + Random.Range(-bulletRandom, bulletRandom);
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(eulerAngles));


            if (shootClip != null)
            {
                shootClip.Play();
            }
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

    public Text t1, t2;

    public override void outOfHealth()
    {
        losePanel.SetActive(true);

        t1.text = "Enemies Killed: " + GameManager.enemyKillCount;
        t2.text = "Bubbles Popped: " + GameManager.bubbleDestroyed;

        if (bar != null)
        {
            Destroy(bar.gameObject);
        }

        Destroy(gameObject);
    }
}
