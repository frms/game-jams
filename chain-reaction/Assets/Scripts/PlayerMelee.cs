using UnityEngine;
using System.Collections;

public class PlayerMelee : MonoBehaviour {
	public float damage = 10f;


	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log (other.tag);

		if (other.tag == "Enemy") {
			other.gameObject.SendMessage("applyDamage", damage);
		}
	}
}
