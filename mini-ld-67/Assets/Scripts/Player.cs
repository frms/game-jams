using UnityEngine;
using System.Collections;

public class Player : Health {

    public Transform heartPrefab;
    public Transform heartsContainer;

    public float thrust;
    public float maxSpeed;
    public float turnSpeed;

    public override float currentHealth
    {
        get
        {
            return base.currentHealth;
        }

        set
        {
            base.currentHealth = value;
            
            for(int i = heartsContainer.childCount; i < base.currentHealth; i++)
            {
                Transform t = Instantiate(heartPrefab) as Transform;
                t.SetParent(heartsContainer, false);
            }

            for (int i = heartsContainer.childCount; i > base.currentHealth; i--)
            {
                Destroy(heartsContainer.GetChild(0).gameObject);
            }
        }
    }

    private Rigidbody rb;

	// Use this for initialization
	public override void Start () {
        base.Start();

        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        float horAxis = Input.GetAxisRaw("Horizontal");
        rb.angularVelocity = new Vector3(0, horAxis * turnSpeed * Mathf.Deg2Rad, 0);

        float vertAxis = Input.GetAxisRaw("Vertical");
        rb.AddForce(transform.right * vertAxis * thrust, ForceMode.Acceleration);

        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Dot")
        {
            Destroy(collision.gameObject);
        }
    }
}
