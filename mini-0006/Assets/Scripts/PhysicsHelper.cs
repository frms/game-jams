using UnityEngine;

public class PhysicsHelper
{
    public static bool isMovingInto(Vector3 dir, Vector3 normal)
    {
        return Vector3.Angle(dir, normal) > 90f;
    }

    public static bool circleCast(Vector2 pos, float radius, Vector2 dir, float dist, out RaycastHit2D hit)
    {
        /* Make sure we don't count collider that we start in */
        bool origQueriesStartInColliders = Physics2D.queriesStartInColliders;
        Physics2D.queriesStartInColliders = false;

        /* Move back by min penetration to hit any colliders that we are already touching */
        Vector2 origin = pos - Physics2D.minPenetrationForPenalty * dir;
        float maxDist = Physics2D.minPenetrationForPenalty + dist;

        hit = Physics2D.CircleCast(origin, radius, dir, maxDist);

        Physics2D.queriesStartInColliders = origQueriesStartInColliders;

        if (hit.collider != null)
        {
            /* Remove the min penetration from the distance. */
            hit.distance -= Physics2D.minPenetrationForPenalty;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static RaycastHit2D[] circleCastAll(Vector2 pos, float radius, Vector2 dir, float dist)
    {
        /* Make sure we don't count collider that we start in */
        bool origQueriesStartInColliders = Physics2D.queriesStartInColliders;
        Physics2D.queriesStartInColliders = false;

        /* Move back by min penetration to hit any colliders that we are already touching */
        Vector2 origin = pos - Physics2D.minPenetrationForPenalty * dir;
        float maxDist = Physics2D.minPenetrationForPenalty + dist;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, dir, maxDist);

        for (int i = 0; i < hits.Length; i++)
        {
            /* Remove the min penetration from the distance. */
            hits[i].distance -= Physics2D.minPenetrationForPenalty;
        }

        Physics2D.queriesStartInColliders = origQueriesStartInColliders;

        return hits;
    }
}
