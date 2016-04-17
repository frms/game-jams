using UnityEngine;
using System.Collections;

public class EnemyBullet : Bullet {
    void OnTriggerEnter(Collider other)
    {
        if ((other.tag != "Enemy"))
        {
            if(other.tag == "Player")
            {
                Health h = other.GetComponent<Health>();
                if (h != null)
                {
                    h.applyDamage(dmg);
                }
            }

            Destroy(gameObject);
        }
    }
}
