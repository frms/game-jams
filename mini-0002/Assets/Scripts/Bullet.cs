using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public float dmg;
    public float speed;

    public Transform target;

    private Vector2 dest;

	// Use this for initialization
	void Start () {
        if (target != null)
        {
            dest =  target.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if((Vector2) transform.position != dest)
        {
            Vector3 newPos = Vector2.MoveTowards(transform.position, dest, Time.deltaTime * speed);
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
        else
        {
            if (target != null)
            {
                target.GetComponent<Health>().applyDamage(dmg);
            }

            Destroy(gameObject);
        }
    }
}
