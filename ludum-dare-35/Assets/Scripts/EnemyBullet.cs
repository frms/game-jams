using UnityEngine;
using System.Collections;

public class EnemyBullet : Bullet {
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer + " " + GameManager.dontSense);
        if ((other.tag != "Enemy") && other.gameObject.layer != GameManager.dontSense)
        {
            if(other.tag == "Player")
            {
                Health h = other.GetComponent<Health>();
                if (h != null)
                {
                    h.applyDamage(dmg);
                }

                Camera.main.GetComponent<CameraMovement>().shake = 0.05f;
            }

            Destroy(gameObject);
        }
    }
}
