using UnityEngine;
using System.Collections;

public class Melee : MonoBehaviour {
	public string hitableTag;
	public float damage = 10f;


	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log (other.tag);

		if (other.tag == hitableTag) {
			other.gameObject.SendMessage("applyDamage", damage);
		}
	}
}
