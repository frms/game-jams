using UnityEngine;
using System.Collections;

public class DontTouch : MonoBehaviour {

    public float atkDmg;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Health h = coll.gameObject.GetComponent<Health>();
            h.applyDamage(transform, atkDmg);
        }
    }

}
