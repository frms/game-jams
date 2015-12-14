using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{
    public float speed = 1.5f;

    [System.NonSerialized]
    public Vector3 direction;

    // Use this for initialization
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.velocity = direction.normalized * speed;
    }

}