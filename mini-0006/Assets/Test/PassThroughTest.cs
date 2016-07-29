using UnityEngine;

[ExecuteInEditMode]
public class PassThroughTest : MonoBehaviour
{

    public Transform start;
    public Transform end;

    public Transform squarePrefab;
    public Transform circlePrefab;

    private float radius = 0.5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 disp = end.position - start.position;

        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        drawLine(disp);

        Color green = new Color(0f, 1f, 0f, 0.35f);
        drawCircle(passThrough(disp), green);
    }

    private void drawLine(Vector3 disp)
    {
        float zRot = Mathf.Atan2(disp.y, disp.x) * Mathf.Rad2Deg;

        Transform line = Instantiate(squarePrefab, start.position + disp / 2f, Quaternion.Euler(0, 0, zRot)) as Transform;
        line.parent = transform;

        Vector3 scale = line.localScale;
        scale.x = disp.magnitude + (2 * radius);
        line.localScale = scale;

        SpriteRenderer sr = line.GetComponent<SpriteRenderer>();
        Color color = sr.color;
        color.a = 0.35f;
        sr.color = color;
    }

    private void drawCircle(Vector3 pos, Color color)
    {
        Transform t = Instantiate(circlePrefab, pos, Quaternion.identity) as Transform;
        t.parent = transform;

        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
        sr.color = color;
    }

    private Vector3 passThrough(Vector3 disp)
    {
        Vector3 result = start.position;

        /* I create an overlap radius that is slightly smaller than the actual radius.
         * I do this because my tests seem to show that Physics2D.circleCast() can 
         * return results where the circle is actually overlapping with a collider
         * by <= Physics2D.minPenetrationForPenalty. And my tests also seem to 
         * suggest that Physics2D.OverlapCircle() will consider a circle overlaping
         * even if it is not overlapping but it is less than Physics2D.minPenetrationForPenalty
         * away from a collider (which matches up what you see with most colliders coming
         * to rest slightly less than Physics2D.minPenetrationForPenalty away from other
         * colliders in the scene). So I must remove Physics2D.minPenetrationForPenalty
         * twice from the radius to make sure our overlap test does not count any colliders
         * that the circle is right up against. */
        float overlapRadius = radius - (2f * Physics2D.minPenetrationForPenalty);

        if (Physics2D.OverlapCircle(end.position, overlapRadius) == null)
        {
            result = end.position;
        }
        else
        {
            RaycastHit2D[] hits = PhysicsHelper.circleCastAll(start.position, radius, disp.normalized, disp.magnitude);

            for (int i = hits.Length - 1; i >= 0; i--)
            {
                if (Physics2D.OverlapCircle(hits[i].centroid, overlapRadius) == null)
                {
                    result = hits[i].centroid;
                    break;
                }
            }
        }

        return result;
    }
}
