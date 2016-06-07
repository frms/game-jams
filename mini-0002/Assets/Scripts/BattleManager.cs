using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public Transform partyPrefab;
    public Transform[] playerCharPrefabs;
    public Transform[] enemyCharPrefabs;

    public Party playerParty;
    public Party enemyParty;

    public string battleName;

    private PlayerCharacter _selected = null;
    public PlayerCharacter selected
    {
        get
        {
            return _selected;
        }
        set
        {
            if (_selected != null)
            {
                _selected.isSelected = false;
            }

            _selected = value;

            if (_selected != null)
            {
                _selected.isSelected = true;
            }
        }
    }

    private static BattleManager _main = null;

    public static BattleManager main
    {
        get { return _main; }
    }

    void Awake()
    {
        if (_main != null && _main != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else {
            _main = this;
        }
    }

    private Vector2 topPartyPos, bottomPartyPos;

    // Use this for initialization
    void Start ()
    {
        Vector3 viewportTop = new Vector3(0.5f, 1f, -Camera.main.transform.position.z);
        Party p = partyPrefab.GetComponent<Party>();
        p.Awake();  // Call Awake manually because the prefab won't ever call it

        topPartyPos = Camera.main.ViewportToWorldPoint(viewportTop);
        topPartyPos.y -= -p.firstSlotPos.y + ( p.slotSize.y / 2 ) + p.slotPadding.y;

        bottomPartyPos = topPartyPos;
        bottomPartyPos.y *= -1;

        GetType().GetMethod(battleName).Invoke(this, null);
    }

    // Update is called once per frame
    void Update ()
    {
        if(selected != null)
        {
            if(!selected.handleInput())
            {
                selected = null;
            }
        }
        else if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "PlayerChar")
            {
                selected = hit.collider.GetComponent<PlayerCharacter>();
            }
        }
	}


    public void battle1()
    {
        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();
        enemyParty.setSlot(enemyParty.numCols / 2, 0, Instantiate(enemyCharPrefabs[0]) as Transform);
        enemyParty.setSlot(0, 1, Instantiate(enemyCharPrefabs[1]) as Transform);
        enemyParty.setSlot(enemyParty.numCols - 1, 1, Instantiate(enemyCharPrefabs[2]) as Transform);

        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot(playerParty.numCols / 2, 0, Instantiate(playerCharPrefabs[0]) as Transform);
        playerParty.setSlot(0, 0, Instantiate(playerCharPrefabs[1]) as Transform);
        playerParty.setSlot(playerParty.numCols - 1, 0, Instantiate(playerCharPrefabs[2]) as Transform);
    }

    public void battle2()
    {
        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();
        enemyParty.setSlot(enemyParty.numCols / 2, 0, Instantiate(enemyCharPrefabs[0]) as Transform);

        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot(playerParty.numCols / 2, 0, Instantiate(playerCharPrefabs[0]) as Transform);
    }
}
