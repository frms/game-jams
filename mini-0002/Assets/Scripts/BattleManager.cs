using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public Transform partyPrefab;
    public Transform playerCharPrefab;
    public Transform enemyCharPrefab;

    public Party playerParty;
    public Party enemyParty;

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

        enemyParty = t.GetComponent<Party>();
        enemyParty.setSlot(enemyParty.numSlots / 2, Instantiate(enemyCharPrefab) as Transform);
    }

    private void createPlayerParty(Vector2 pos)
    {
        Transform t = Instantiate(partyPrefab, pos, Quaternion.identity) as Transform;
        t.name = "PlayerParty";

        playerParty = t.GetComponent<Party>();
        playerParty.setSlot(playerParty.numSlots / 2, Instantiate(playerCharPrefab) as Transform);
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
}
