using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 8.413461538f;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        dir.Normalize();
        rb.velocity = dir * speed;

        if(dir.magnitude > 0f)
        {
            rb.rotation = Mathf.Atan2(dir[1], dir[0]) * Mathf.Rad2Deg;
        }
    }
}
