using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
	public float distance = 10f;

	private LineRenderer gunLine;
	private int damageableLayer;
	
	// Use this for initialization
	void Start () {
		gunLine = GetComponent<LineRenderer> ();
		damageableLayer = 1 << LayerMask.NameToLayer ("Damageable");
	}
	
	public void use() {
		Vector3 endPoint = transform.position + transform.right.normalized * distance;

		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		gunLine.SetPosition (1, endPoint);

		RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, transform.right, distance, damageableLayer);
		Debug.Log (hits.Length);
	}
}
