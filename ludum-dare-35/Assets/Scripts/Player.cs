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

    // Use this for initialization
    public override void Start()
    {
        rb = GetComponent<MovementAIRigidbody>();
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

        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 eulerAngles = bulletPrefab.eulerAngles;
            eulerAngles.y = transform.eulerAngles.y;
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(eulerAngles));
        }

        // Make the  bar scale to the current barProgress
        bar.localScale = new Vector3(percentHealth * barSize.x, barSize.y, barSize.z);
    }

    void FixedUpdate()
    {
        rotateChar();
        moveChar();
    }

    private void moveChar()
    {
        Vector3 vel = (transform.right * vertAxis) + (transform.forward * sideStepDir);
        vel = vel.normalized * speed;
        rb.velocity = vel;
    }

    private void rotateChar()
    {
        rb.angularVelocity = horAxis * turnSpeed * Mathf.Deg2Rad;
        // Clear out any x/z orientation
        rb.rotation = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0);
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
