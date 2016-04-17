using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public Transform target;

    //public float t;
    //private Rigidbody rb;


	private Vector3 displacement;
	// Use this for initialization
	void Start () {
        //target = GameObject.Find ("Player").transform;
        //rb = target.GetComponent<Rigidbody>();

		displacement = transform.position - target.position;
	}

    public float shake = 0;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;


    // LateUpdate is called once per frame after the other normal Update functions have already run
    void LateUpdate () {
		//Debug.Log (Vector3.Distance (transform.position, target.position));
		if(target != null) {

            if (shake > 0)
            {
                transform.position = transform.position = target.position + displacement + Random.insideUnitSphere * shakeAmount;
                shake -= Time.deltaTime * decreaseFactor;
            }
            else {
                shake = 0.0f;

                transform.localPosition = Vector3.zero;

                transform.position = target.position + displacement;
                //Vector3 targetPos = rb.position + rb.velocity.normalized * 3;
                //Vector3 temp = transform.position;
                //temp.x = Mathf.Lerp(transform.position.x, targetPos.x, t * Time.deltaTime);
                //temp.z = Mathf.Lerp(transform.position.z, targetPos.z, t * Time.deltaTime);
                //transform.position = temp;
            }
        }
	}
}
