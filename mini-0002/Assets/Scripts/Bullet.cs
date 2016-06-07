using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public float dmg;
    public float speed;

    public Transform target;

    private int targetLayer;

    // Use this for initialization
    void Start()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            targetLayer = target.gameObject.layer;
        }
    }

    private Vector2 dest;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            dest = target.position;
        }

        if ((Vector2)transform.position != dest)
        {
            Vector3 newPos = Vector2.MoveTowards(transform.position, dest, Time.deltaTime * speed);
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
        else
        {
            if(target != null)
            {
                target.GetComponent<Health>().applyDamage(dmg);
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == targetLayer)
        {
            other.GetComponent<Health>().applyDamage(dmg);
            Destroy(gameObject);
        }
    }
}
