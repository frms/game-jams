﻿using UnityEngine;
using System.Collections;

public class PlatformerCamera : MonoBehaviour
{
    public Player target;
    public float speed = 10f;

    public enum NumberType { units, percentage }
    public NumberType kindOfNumbers = NumberType.percentage;

    public float platformSnapOffset = (1f / 6f);
    public float lookUpDownOffset = 0.25f;
    public float height = 0.75f;
    public float innerWidth = 0.1f;
    public float sideWidth = 0.11f;

    public Transform debugSquarePrefab;

    internal bool lookUp;
    internal bool lookDown;

    private Vector2 screenSize;

    private float facing;

    void Start ()
    {
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, -Camera.main.transform.position.z));
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, -Camera.main.transform.position.z));
        screenSize = topRight - bottomLeft;

        if(kindOfNumbers == NumberType.percentage)
        {
            platformSnapOffset *= screenSize.y;
            lookUpDownOffset *= screenSize.y;
            height *= screenSize.y;
            innerWidth *= screenSize.x;
            sideWidth *= screenSize.x;
        }

        facing = target.facing;
    }

    private float seekInner = 0;

	void LateUpdate ()
    {
        Vector3 pos = transform.position;
        Vector3 targetPos = target.transform.position;

        /* X Direction */

        float targetLeft = targetPos.x - (target.size.x / 2f);
        float targetRight = targetPos.x + (target.size.x / 2f);

        float innerLeft = transform.position.x - (innerWidth * 0.5f);
        float innerRight = transform.position.x + (innerWidth * 0.5f);
        float outerLeft = innerLeft - sideWidth;
        float outerRight = innerRight + sideWidth;

        /* Seek an inner bound if the target goes past an outer bound */

        if (targetLeft < outerLeft)
        {
            seekInner = innerWidth * -0.5f;
            facing = -1;
        }

        if (targetRight > outerRight)
        {
            seekInner = innerWidth * 0.5f;
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
        /* Move the camera forward if the character is walking into an inner
         * bound in the correct direction */
        else
        {
            if(facing < 0 && innerRight > targetPos.x)
            {
                pos.x = targetPos.x + innerWidth * -0.5f;
            }
            if (facing > 0 && innerLeft < targetPos.x)
            {
                pos.x = targetPos.x + innerWidth * 0.5f;
            }
        }

        /* Y Direction */

        float yOffset = 0;

        if (lookUp)
        {
            yOffset = lookUpDownOffset;
        }

        if (lookDown)
        {
            yOffset = -lookUpDownOffset;
        }

        /* Platform Snaping */
        if (target.isTouchingGround)
        {
            /* When I was using the target's transform position I would get a small
             * jitter on platform snapping landing. I think this is due to the character
             * being interpolated (and thus the rb of the char is desync and ahead of 
             * the transform pos of the char). So I think the code was finding out that
             * it is on ground (in the physics update loop) before the game was reflecting
             * it since the char was set to interpolation. So when I tried snapping to
             * the foot of the transform pos it would have a few frames of movement when
             * it should already be "snapped". Instead I'm using the rigidbody position 
             * which is not causing any jitter on snap landing. Of course normally if I
             * were to use the rigidbody position in this LateUpdate loop I would see
             * jitter since LateUpdate is refreshing more often then FixedUpdate. But
             * in this case it won't cause any problems since I'm moving towards the 
             * position rather than instantly setting my y coord based on the rigidbody
             * pos. I'm not %100 sure if my thoughts on the original problem are actually
             * correct. I did not want to spend time to really dig in and verify.
             * Also it will be annoying to verify since turning Interpolation off
             * temporarily will cause jitter from the fact that LateUpdate is desynced
             * from FixedUpdate. So I'd have to verify it in some manner that would avoid
             * other sources of jitter. */
            float targetBottom = target.rigidbodyPos.y - (target.size.y / 2f);
            pos.y = moveTowards(pos.y, targetBottom + yOffset + platformSnapOffset);
        }
        /* Keep the target within Y bounds */
        else
        {
            float targetBottom = targetPos.y - (target.size.y / 2f);
            float targetTop = targetPos.y + (target.size.y / 2f);

            float camBottom = transform.position.y - (height / 2f);
            float camTop = transform.position.y + (height / 2f);

            if (targetBottom < camBottom)
            {
                pos.y += targetBottom - camBottom;
            }

            if (targetTop > camTop)
            {
                pos.y += targetTop - camTop;
            }
        }

        transform.position = pos;

        debugDraw();
    }

    private float moveTowards(float s, float e)
    {
        return Mathf.MoveTowards(s, e, speed * Time.deltaTime);
    }

    private Transform[] debugSquares;

    private void debugDraw()
    {
        /* Draw nothing if there is no prefab */
        if(debugSquarePrefab == null)
        {
            return;
        }

        if(debugSquares == null)
        {
            debugSquares = new Transform[5];
            for (int i = 0; i < 5; i++)
            {
                debugSquares[i] = Instantiate(debugSquarePrefab) as Transform;
            }
        }

        float innerLeft = transform.position.x - (innerWidth * 0.5f);
        float innerRight = transform.position.x + (innerWidth * 0.5f);
        float outerLeft = innerLeft - sideWidth;
        float outerRight = innerRight + sideWidth;

        Vector2 start, end;

        /* Platform snapping */
        start.x = transform.position.x - (screenSize.x * 0.3f);
        start.y = transform.position.y - platformSnapOffset;
        end.x = transform.position.x + (screenSize.x * 0.3f);
        end.y = start.y;
        drawLine(start, end, debugSquares[0]);

        /* Inner Left */
        start.x = innerLeft;
        start.y = transform.position.y - (screenSize.y * 0.35f);
        end.x = innerLeft;
        end.y = transform.position.y + (screenSize.y * 0.35f);
        drawLine(start, end, debugSquares[1]);

        /* Inner Right */
        start.x = innerRight;
        end.x = innerRight;
        drawLine(start, end, debugSquares[2]);

        /* Outer Left */
        start.x = outerLeft;
        start.y = transform.position.y - (screenSize.y * 0.25f);
        end.x = outerLeft;
        end.y = transform.position.y + (screenSize.y * 0.25f);
        drawLine(start, end, debugSquares[3]);

        /* Outer Right */
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
}
