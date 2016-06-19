using UnityEngine;
using System.Collections;

public class PlatformerCamera : MonoBehaviour
{
    public Player target;
    public float speed;

    public Transform squarePrefab;

    private Vector2 screenSize;
    private float platformOffset;
    private float innerXSize;
    private float outerXSize;
    private float facing;

    private Transform[] debugSquares;

	// Use this for initialization
	void Start ()
    {
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, -Camera.main.transform.position.z));
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, -Camera.main.transform.position.z));
        screenSize = topRight - bottomLeft;

        platformOffset = screenSize.y * 0.2f;
        innerXSize = screenSize.x * 0.1f;
        outerXSize = screenSize.x * 0.11f;

        facing = target.facing;

        debugSquares = new Transform[5];
        for(int i = 0; i < 5; i++)
        {
            debugSquares[i] = Instantiate(squarePrefab) as Transform;
        }
    }

    private float seekInner = 0;

	void LateUpdate ()
    {
        float startX = transform.position.x;

        float innerLeft = transform.position.x - (innerXSize * 0.5f);
        float innerRight = transform.position.x + (innerXSize * 0.5f);
        float outerLeft = innerLeft - outerXSize;
        float outerRight = innerRight + outerXSize;

        Vector3 pos = transform.position;

        Vector3 targetPos = target.transform.position;
        float targetLeft = targetPos.x - (target.size.x / 2f);
        float targetRight = targetPos.x + (target.size.x / 2f);

        if (targetLeft < outerLeft)
        {
            seekInner = innerXSize * -0.5f;
            facing = -1;
        }

        if (targetRight > outerRight)
        {
            seekInner = innerXSize * 0.5f;
            facing = 1;
        }

        if(seekInner != 0)
        {
            float endX = targetPos.x + seekInner;
            pos.x = moveTowards(pos.x, endX);

            if(pos.x == endX)
            {
                seekInner = 0;
            }
        }
        else
        {
            if(facing < 0 && innerRight > targetPos.x)
            {
                pos.x = targetPos.x + innerXSize * -0.5f;
            }
            if (facing > 0 && innerLeft < targetPos.x)
            {
                pos.x = targetPos.x + innerXSize * 0.5f;
            }
        }


        // Y code
        if (target.isTouchingGround)
        {
            float targetBottom = target.transform.position.y - (target.size.y / 2f);
            pos.y = moveTowards(pos.y, targetBottom + platformOffset);
        }

        transform.position = pos;

        innerLeft += -startX + transform.position.x;
        innerRight += -startX + transform.position.x;
        outerLeft += -startX + transform.position.x;
        outerRight += -startX + transform.position.x;
        debugDraw(innerLeft, innerRight, outerLeft, outerRight);
    }

    private void debugDraw(float innerLeft, float innerRight, float outerLeft, float outerRight)
    {
        Vector2 start, end;

        // Platform snapping 
        start.x = transform.position.x - (screenSize.x * 0.3f);
        start.y = transform.position.y - platformOffset;
        end.x = transform.position.x + (screenSize.x * 0.3f);
        end.y = start.y;
        drawLine(start, end, debugSquares[0]);

        // Inner Left 
        start.x = innerLeft;
        start.y = transform.position.y - (screenSize.y * 0.35f);
        end.x = innerLeft;
        end.y = transform.position.y + (screenSize.y * 0.35f);
        drawLine(start, end, debugSquares[1]);

        // Inner Right 
        start.x = innerRight;
        end.x = innerRight;
        drawLine(start, end, debugSquares[2]);

        // Outer Left 
        start.x = outerLeft;
        start.y = transform.position.y - (screenSize.y * 0.25f);
        end.x = outerLeft;
        end.y = transform.position.y + (screenSize.y * 0.25f);
        drawLine(start, end, debugSquares[3]);

        // Outer Right 
        start.x = outerRight;
        end.x = outerRight;
        drawLine(start, end, debugSquares[4]);
    }

    private void drawLine(Vector2 start, Vector2 end, Transform square)
    {
        //Debug.DrawLine(start, end, Color.red, 0f, false);

        Vector2 pos = (start + end) / 2f;
        square.position = pos;

        Vector3 scale = Vector3.one;

        Vector2 diff = end - start;
        if(Mathf.Abs(diff.x) < Mathf.Abs(diff.y))
        {
            scale.x = 0.1f;
            scale.y = Mathf.Abs(diff.y);
        }
        else
        {
            scale.x = Mathf.Abs(diff.x);
            scale.y = 0.1f;
        }

        square.localScale = scale;
    }

    private float moveTowards(float s, float e)
    {
        return Mathf.MoveTowards(s, e, speed * Time.deltaTime);
    }
}
