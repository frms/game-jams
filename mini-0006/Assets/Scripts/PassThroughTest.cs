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

        if (Physics2D.OverlapCircle(end.position, radius) == null)
        {
            result = end.position;
        }
        else
        {
            RaycastHit2D[] hits = circleCastAll(start.position, radius, disp.normalized, disp.magnitude);

            for (int i = hits.Length - 1; i >= 0; i--)
            {
                if (Physics2D.OverlapCircle(hits[i].centroid, radius) == null)
                {
                    result = hits[i].centroid;
                    break;
                }
            }
        }

        return result;
    }

    private RaycastHit2D[] circleCastAll(Vector2 pos, float radius, Vector2 dir, float dist)
    {
        /* Make sure we don't count collider that we start in */
        bool origQueriesStartInColliders = Physics2D.queriesStartInColliders;
        Physics2D.queriesStartInColliders = false;

        Vector2 origin = pos - Physics2D.minPenetrationForPenalty * dir;
        float maxDist = Physics2D.minPenetrationForPenalty + dist;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, dir, maxDist);

        for (int i = 0; i < hits.Length; i++)
        {
            /* I'm not sure why but it seems that I need to subtract the
             * Physics2D.minPenetrationForPenalty twice (instead of what should be once).
             * It seems like in Physics 2D the min penetration is more like a min
             * separation and overlap is never allowed. */
            hits[i].distance -= 2 * Physics2D.minPenetrationForPenalty;
            hits[i].centroid -= (2 * Physics2D.minPenetrationForPenalty) * dir;
        }

        Physics2D.queriesStartInColliders = origQueriesStartInColliders;

        return hits;
    }
}
