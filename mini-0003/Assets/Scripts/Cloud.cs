using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour
{
    public Vector2 speed;
    public float chanceToMove;

    private bool isMoving;
    private float actualSpeed;
    private CloudBox cb;

	// Use this for initialization
	void Start ()
    {
        isMoving = (Random.value <= chanceToMove);
        actualSpeed = Random.Range(speed.x, speed.y);

        cb = transform.parent.GetComponent<CloudBox>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(isMoving)
        {
            transform.position += (Vector3) (Vector2.left * actualSpeed * Time.deltaTime);

            if(transform.position.x < cb.bottomLeft.x)
            {
                Vector3 pos = transform.position;
                pos.x = cb.topRight.x;
                transform.position = pos;
            }
        }
	}
}
