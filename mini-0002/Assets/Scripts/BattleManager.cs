using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {
    public Transform partyPrefab;

	// Use this for initialization
	void Start () {
        Vector3 viewportTop = new Vector3(0.5f, 1f, -Camera.main.transform.position.z);
        Party p = partyPrefab.GetComponent<Party>();

        Vector2 partyPos = Camera.main.ViewportToWorldPoint(viewportTop);
        partyPos.y -= ( p.slotSize.y / 2 ) + p.slotPadding.y;

        Instantiate(partyPrefab, partyPos, Quaternion.identity);

        partyPos.y *= -1;

        Instantiate(partyPrefab, partyPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
