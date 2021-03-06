﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

    public static BattleManager main;

    void Awake()
    {
        if (main != null && main != this)
        {
            DestroyImmediate(gameObject);
            return;
        }
        else {
            DontDestroyOnLoad(gameObject);
            main = this;
        }
    }

    void Start()
    {
        if(Application.isEditor)
        {
            setUp();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        setUp();
    }

    private Vector2 topPartyPos, bottomPartyPos;

    private void setUp() {
        pauseOverlay = GameObject.Find("PauseOverlay");

        Vector3 viewportTop = new Vector3(0.5f, 1f, -Camera.main.transform.position.z);
        Party p = partyPrefab.GetComponent<Party>();
        p.setUp();  // Call setUp() manually because the prefab won't ever call it

        topPartyPos = Camera.main.ViewportToWorldPoint(viewportTop);
        topPartyPos.y -= -p.firstSlotPos.y + ( p.slotSize.y / 2 ) + p.slotPadding.y;

        bottomPartyPos = topPartyPos;
        bottomPartyPos.y *= -1;

        if(playerParty == null)
        {
            initialPlayerParty();
        }

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

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("BattleScene");
        }
	}

    private Transform createSingleTarget(Transform prefab, float hp = -1, float rate = -1, Transform target = null)
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


    public void initialPlayerParty()
    {
        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot(playerParty.getRandomEmptySlot(), createSingleTarget(playerCharPrefabs[0]));
        //playerParty.setSlot(playerParty.getRandomEmptySlot(), createSingleTarget(playerCharPrefabs[1]));
        //playerParty.setSlot(playerParty.getRandomEmptySlot(), Instantiate(playerCharPrefabs[2]) as Transform);

        DontDestroyOnLoad(ppt.gameObject);
    }

    public void battleAll()
    {
        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();
        enemyParty.setSlot(enemyParty.getRandomEmptySlot(), createSingleTarget(enemyCharPrefabs[0]));
        enemyParty.setSlot(enemyParty.getRandomEmptySlot(), createSingleTarget(enemyCharPrefabs[1]));
        enemyParty.setSlot(enemyParty.getRandomEmptySlot(), Instantiate(enemyCharPrefabs[2]) as Transform);
    }

    /// <summary>
    /// Defeat the healer first or die
    /// </summary>
    public void battle1()
    {
        float ppDps = playerParty.getDps();
        float ppHp = playerParty.getHp();

        /* Min dmg the player party will take in this fight */
        float minDmgPercent = 0.1f;

        /* Max dmg the player party will take in this fight */
        float maxDmgPercent = 0.85f;

        /* Constants 1 and 3 are magic constants that I picked to help determine the length of the battle */
        float ehHp = 1 * ppDps;
        float edHp = 3 * ppDps;

        float tmin = (ehHp + edHp) / ppDps;

        float edDps = (minDmgPercent * ppHp ) / tmin;

        float tmax = (maxDmgPercent * ppHp) / edDps;

        float ehDps = (edHp / tmax) - ppDps;

        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();
        Transform e1 = createSingleTarget(enemyCharPrefabs[0], edHp, 10 / edDps);
        enemyParty.setSlot(enemyParty.getRandomEmptySlot(), e1);
        Transform e2 = createSingleTarget(enemyCharPrefabs[1], ehHp, -10 / ehDps, e1);
        enemyParty.setSlot(enemyParty.getRandomEmptySlot(), e2);
    }

    /// <summary>
    /// Defeat the enemy in the back first to win (move your char to get a clear shot).
    /// </summary>
    public void battle2()
    {
        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot(playerParty.numCols / 2, 0, createSingleTarget(playerCharPrefabs[0]));

        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();

        int x = playerParty.getRandomCharSlot().x;

        Transform e1 = createSingleTarget(enemyCharPrefabs[0], 50, 0.6f);
        enemyParty.setSlot(x, 1, e1);
        Transform e2 = createSingleTarget(enemyCharPrefabs[0], 150, 2f);
        enemyParty.setSlot(x, 0, e2);
    }

    /// <summary>
    /// Defeat the weaker/higher dmg enemies first.
    /// </summary>
    public void battle3()
    {
        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot(playerParty.numCols / 2, 0, createSingleTarget(playerCharPrefabs[0]));

        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();

        int x = Random.Range(1, enemyParty.numCols - 1);

        Transform e1 = createSingleTarget(enemyCharPrefabs[0], 25, 1.2f);
        enemyParty.setSlot(x - 1, 1, e1);

        Transform e2 = createSingleTarget(enemyCharPrefabs[0], 125, 3f);
        enemyParty.setSlot(x, 0, e2);

        Transform e3 = createSingleTarget(enemyCharPrefabs[0], 25, 1.2f);
        enemyParty.setSlot(x + 1, 1, e3);
    }

    /// <summary>
    /// One big bad enemy will lock on to a player character. The player will need to move around his party so the damage gets distributed across the whole party rather than just 1 char.
    /// </summary>
    public void battle4()
    {
        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot((playerParty.numCols / 2) - 1, 0, createSingleTarget(playerCharPrefabs[0], 50));
        playerParty.setSlot(playerParty.numCols / 2, 0, createSingleTarget(playerCharPrefabs[0], 50));
        playerParty.setSlot((playerParty.numCols / 2) + 1, 0, createSingleTarget(playerCharPrefabs[0], 50));

        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();
        enemyParty.setSlot(enemyParty.getRandomEmptySlot(), createSingleTarget(enemyCharPrefabs[0], 500, 1f));
    }

    /// <summary>
    /// The enemy will target your weakest character and the player needs to move its biggest player in the way of the atks.
    /// </summary>
    public void battle5()
    {
        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot((playerParty.numCols / 2) - 1, 0, createSingleTarget(playerCharPrefabs[0], 30));
        playerParty.setSlot(playerParty.numCols / 2, 0, createSingleTarget(playerCharPrefabs[0], 170));

        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();
        enemyParty.setSlot(enemyParty.getRandomEmptySlot(), createSingleTarget(enemyCharPrefabs[0], 400, 1f, playerParty.getWeakestChar()));
    }

    /// <summary>
    /// The enemy will randomly attack the player party, but the player party has many weaker chars that need to be protected.
    /// </summary>
    public void battle6()
    {
        Transform ppt = Instantiate(partyPrefab, bottomPartyPos, Quaternion.identity) as Transform;
        ppt.name = "PlayerParty";

        playerParty = ppt.GetComponent<Party>();
        playerParty.setSlot(0, 0, createSingleTarget(playerCharPrefabs[0], 30));
        playerParty.setSlot(1, 0, createSingleTarget(playerCharPrefabs[0], 30));
        playerParty.setSlot(2, 0, createSingleTarget(playerCharPrefabs[0], 170));
        playerParty.setSlot(3, 0, createSingleTarget(playerCharPrefabs[0], 170));
        playerParty.setSlot(4, 0, createSingleTarget(playerCharPrefabs[0], 30));
        playerParty.setSlot(5, 0, createSingleTarget(playerCharPrefabs[0], 30));


        Transform ept = Instantiate(partyPrefab, topPartyPos, Quaternion.identity) as Transform;
        ept.name = "EnemyParty";

        enemyParty = ept.GetComponent<Party>();
        Transform e = createSingleTarget(enemyCharPrefabs[0], 600, 0.25f);
        e.GetComponent<EnemyDmgDealer>().newTargetEachAtk = true;
        enemyParty.setSlot(enemyParty.getRandomEmptySlot(), e);
    }
}
