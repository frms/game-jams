using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {
    public Transform partyPrefab;
    public Transform playerCharPrefab;
    public Transform enemyCharPrefab;

	// Use this for initialization
	void Start ()
    {
        Vector3 viewportTop = new Vector3(0.5f, 1f, -Camera.main.transform.position.z);
        Party p = partyPrefab.GetComponent<Party>();

        Vector2 partyPos = Camera.main.ViewportToWorldPoint(viewportTop);
        partyPos.y -= ( p.slotSize.y / 2 ) + p.slotPadding.y;

        createEnemyParty(partyPos);

        partyPos.y *= -1;
        createPlayerParty(partyPos);
    }

    private void createEnemyParty(Vector2 pos)
    {
        Transform t = Instantiate(partyPrefab, pos, Quaternion.identity) as Transform;
        t.name = "EnemyParty";

        Party p = t.GetComponent<Party>();
        p.setSlot(p.numSlots / 2, Instantiate(enemyCharPrefab) as Transform);
    }

    private void createPlayerParty(Vector2 pos)
    {
        Transform t = Instantiate(partyPrefab, pos, Quaternion.identity) as Transform;
        t.name = "PlayerParty";

        Party p = t.GetComponent<Party>();
        p.setSlot(p.numSlots / 2, Instantiate(playerCharPrefab) as Transform);
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
