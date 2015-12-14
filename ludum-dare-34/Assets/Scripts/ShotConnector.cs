using UnityEngine;
using System.Collections;

public class ShotConnector : MonoBehaviour {

    public Transform shot1;
    public Transform shot2;

    public float padding = 0;

    private float shotDiameter;

    // Use this for initialization
    void Start () {
        SphereCollider shotCol = shot1.GetComponent<SphereCollider>();
        shotDiameter = Mathf.Max(shot1.localScale.x, shot1.localScale.y, shot1.localScale.z) * shotCol.radius * 2f;
    }
	
	void LateUpdate () {
        Vector3 displacement = shot1.position - shot2.position;

        transform.rotation = Quaternion.FromToRotation(Vector3.up, displacement.normalized);

        transform.position = (shot1.position + shot2.position) / 2;

        Vector3 scale = transform.localScale;
        scale.y = (displacement.magnitude - shotDiameter - 2*padding) / 2;
        transform.localScale = scale;
    }
}
