using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour {
    public float[] speed;

    private Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();

        Vector2 dir = Random.insideUnitCircle.normalized;
        rb.velocity = new Vector3(dir.x, 0, dir.y) * Random.Range(speed[0], speed[1]);
	}
}
