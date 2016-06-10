using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public GameObject pauseOverlay;

    public bool isPaused
    {
        get
        {
            return pauseOverlay.activeSelf;
        }
        set
        {
            pauseOverlay.SetActive(value);
        }
    }

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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused;
        }
	}

    private Transform createSingleTargetChar(Transform prefab, float hp = -1, float rate = -1, Transform target = null)
    {
        Transform t = Instantiate(prefab) as Transform;

        if(hp != -1)
        {
            Health h = t.GetComponent<Health>();
            h.currentHealth = hp;
            h.maxHealth = hp;
        }

        SingleTarget st = t.GetComponent<SingleTarget>();

        if(rate != -1)
        {
            st.rate = rate;
        }

        if(target != null)
        {
            st.target = target;
        }

        return t;
    }


    public void battleAll()
    {
        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot(playerParty.numCols / 2, 0, createSingleTargetChar(playerCharPrefabs[0]));
        playerParty.setSlot(0, 0, createSingleTargetChar(playerCharPrefabs[1]));
        playerParty.setSlot(playerParty.numCols - 1, 0, Instantiate(playerCharPrefabs[2]) as Transform);

        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();
        enemyParty.setSlot(enemyParty.numCols / 2, 0, createSingleTargetChar(enemyCharPrefabs[0]));
        enemyParty.setSlot(0, 1, createSingleTargetChar(enemyCharPrefabs[1]));
        enemyParty.setSlot(enemyParty.numCols - 1, 1, Instantiate(enemyCharPrefabs[2]) as Transform);
    }

    /// <summary>
    /// Defeat the healer first or die
    /// </summary>
    public void battle1()
    {
        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot(playerParty.numCols / 2, 0, createSingleTargetChar(playerCharPrefabs[0]));

        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();
        Transform e1 = createSingleTargetChar(enemyCharPrefabs[0], 120, 1);
        enemyParty.setSlot(enemyParty.numCols / 2, 1, e1);
        Transform e2 = createSingleTargetChar(enemyCharPrefabs[1], 50, 1, e1);
        enemyParty.setSlot((enemyParty.numCols / 2) - 1, 1, e2);
    }

    /// <summary>
    /// Defeat the enemy in the back first to win (move your char to get a clear shot).
    /// </summary>
    public void battle2()
    {
        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot(playerParty.numCols / 2, 0, createSingleTargetChar(playerCharPrefabs[0]));

        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();
        Transform e1 = createSingleTargetChar(enemyCharPrefabs[0], 50, 0.6f);
        enemyParty.setSlot(enemyParty.numCols / 2, 1, e1);
        Transform e2 = createSingleTargetChar(enemyCharPrefabs[0], 150, 3f);
        enemyParty.setSlot(enemyParty.numCols / 2, 0, e2);
    }

    /// <summary>
    /// Defeat the weaker/higher dmg enemies first.
    /// </summary>
    public void battle3()
    {
        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot(playerParty.numCols / 2, 0, createSingleTargetChar(playerCharPrefabs[0]));

        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();
        Transform e1 = createSingleTargetChar(enemyCharPrefabs[0], 25, 1.2f);
        enemyParty.setSlot((enemyParty.numCols / 2) - 1, 1, e1);

        Transform e2 = createSingleTargetChar(enemyCharPrefabs[0], 125, 3f);
        enemyParty.setSlot(enemyParty.numCols / 2, 0, e2);

        Transform e3 = createSingleTargetChar(enemyCharPrefabs[0], 25, 1.2f);
        enemyParty.setSlot((enemyParty.numCols / 2) + 1, 1, e3);
    }
}
