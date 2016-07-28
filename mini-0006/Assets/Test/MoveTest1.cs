using UnityEngine;

public class MoveTest1 : MonoBehaviour
{
    public float speed = 8.413461538f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if(transform.position.x > 12)
        {
            Vector3 pos = transform.position;
            pos.x -= 24;
            transform.position = pos;
        }
    }
}
