using UnityEngine;
using System.Collections;

public class StompSensor : MonoBehaviour {
    public float dmg;

    private Rigidbody2D rb;

    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && rb.velocity.y <= 0)
        {
            float enemyTop = (other.transform.localScale.y / 2f) + other.transform.position.y;

            if(transform.position.y > enemyTop)
            {
                Health h = other.GetComponent<Health>();
                h.applyDamage(transform, dmg);
            }
        }
    }
}
