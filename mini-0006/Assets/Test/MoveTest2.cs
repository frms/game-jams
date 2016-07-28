using UnityEngine;

public class MoveTest2 : MonoBehaviour
{

    public float speed = 8.413461538f;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 12)
        {
            Vector3 pos = transform.position;
            pos.x -= 24;
            transform.position = pos;
        }
    }
}
