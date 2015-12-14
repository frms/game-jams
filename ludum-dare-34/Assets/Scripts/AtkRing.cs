using UnityEngine;
using System.Collections;

public class AtkRing : MonoBehaviour {

    public Transform shotPrefab;
    public Transform connectorPrefab;

    public int numOfShots = 10;
    public float radius = 0.15f;

    private Shot[] shots;
    private ShotConnector[] connectors;

	// Use this for initialization
	void Start () {
        shots = new Shot[numOfShots];
        connectors = new ShotConnector[numOfShots];

	    for(int i = 0; i < numOfShots; i++)
        {
            float angle = (2 * Mathf.PI / numOfShots) * i;

            Vector3 dir = Vector3.zero;
            dir.x = Mathf.Cos(angle);
            dir.z = Mathf.Sin(angle);

            Shot s = makePrefab(shotPrefab, dir* radius).GetComponent<Shot>();
            s.direction = dir;
            shots[i] = s;
            
            connectors[i] = makePrefab(connectorPrefab).GetComponent<ShotConnector>();
        }

        for(int i = 0; i < numOfShots; i++)
        {
            connectors[i].shot1 = shots[i].transform;
            connectors[i].shot2 = shots[(i+1)%numOfShots].transform;
        }
	}

    private Transform makePrefab(Transform prefab, Vector3 position = default(Vector3))
    {
        Transform child = Instantiate(prefab, Vector3.zero, Quaternion.identity) as Transform;
        child.parent = transform;
        child.localPosition = position;

        return child;
    }
	
}
