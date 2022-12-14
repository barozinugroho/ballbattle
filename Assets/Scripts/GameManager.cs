using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState gameState;

    public GameObject soldier;
    public GameObject landTop, landBottom;
    public GameObject ball;

    public Mode attacker;
    public SoldierParam attackerParam;

    public Mode defender;
    public SoldierParam defenderParam;

    public ListSoldier listSoldier;
    public MatchManager match;

    public const string ATTACKER_TAG = "Attacker";
    public const string DEFENDER_TAG = "Defender";

    public int maxHP;
    public float timeLimits;
    public float timer { get; private set; }

    private bool isAttackersTurn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        listSoldier = GetComponent<ListSoldier>();
        listSoldier.freeAttackers = new List<Transform>();
        listSoldier.freeDefenders = new List<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.MAIN_MENU;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.PLAY)
        {
            OnTap();
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                match.UpdateMatchResult("draw");
            }
        }
    }

    private void OnTap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, 1 << 6))
            {
                switch (hit.collider.tag)
                {
                    case ATTACKER_TAG:
                        if (attacker.energy >= attackerParam.energyCost)
                        {
                            SpawnSoldier(hit.point, hit.collider);
                            attacker.ui.ReduceEnergy(attackerParam.energyCost, (int)attacker.energy);
                            isAttackersTurn = false;
                        }
                        break;
                    case DEFENDER_TAG:
                        if (defender.energy >= defenderParam.energyCost)
                        {
                            SpawnSoldier(hit.point, hit.collider);
                            defender.ui.ReduceEnergy(defenderParam.energyCost, (int)defender.energy);
                            isAttackersTurn = true;
                        }
                        break;
                }
                

                //Debug.Log($"mousePos: {Input.mousePosition}");
                //Debug.Log($"ScreenTOWorldPoint: {Camera.main.ScreenToWorldPoint(Input.mousePosition)}");
                Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), hit.point, Color.red);
            }
        }
    }

    private void SpawnSoldier(Vector3 _pos, Collider _col)
    {
        _pos.y += 1f;
        GameObject go;
        switch (_col.tag)
        {
            case ATTACKER_TAG:
                attacker.energy -= attackerParam.energyCost;
                go = Instantiate(soldier, _pos, Quaternion.LookRotation(defender.land.transform.position));
                go.transform.SetParent(transform);
                go.GetComponent<Soldier>().param = attackerParam;
                //listSoldier.freeAttackers.Add(go.transform);
                break;
            case DEFENDER_TAG:
                defender.energy -= defenderParam.energyCost;
                go = Instantiate(soldier, _pos, Quaternion.LookRotation(attacker.land.transform.position));
                go.transform.SetParent(transform);
                go.GetComponent<Soldier>().param = defenderParam;
                listSoldier.freeDefenders.Add(go.transform);
                break;
        }
    }

    private void RemoveSoldier()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        listSoldier.freeAttackers = new List<Transform>();
        listSoldier.freeDefenders = new List<Transform>();
    }

    public void SwitchMode()
    {
        isAttackersTurn = true;
        Mode temp = attacker;
        attacker = defender;
        defender = temp;

        RemoveSoldier();
        SetupPosition(attacker.land, defender.land);
    }

    private void SetupPosition(GameObject _attackerPos, GameObject _defenderPos)
    {
        _attackerPos.tag = ATTACKER_TAG;
        _attackerPos.GetComponent<Land>().ChangeMat();

        _defenderPos.tag = DEFENDER_TAG;
        _defenderPos.GetComponent<Land>().ChangeMat();

        attacker.energy = 0;
        attacker.land = _attackerPos;
        attacker.ui = attacker.land.GetComponent<Land>().uiPlayer;
        attacker.ui.ResetUIEnergyBar();
        attacker.ui.UpdateName(ATTACKER_TAG);
        attacker.ui.tag = ATTACKER_TAG;

        defender.energy = 0;
        defender.land = _defenderPos;
        defender.ui = defender.land.GetComponent<Land>().uiPlayer;
        defender.ui.ResetUIEnergyBar();
        defender.ui.UpdateName(DEFENDER_TAG);
        defender.ui.tag = DEFENDER_TAG;

        timer = timeLimits;
        SpawnBall(attacker.land);
    }

    public bool isBallOccupied;
    public GameObject spawnedBall;
    public void SpawnBall(GameObject _attacker)
    {
        isBallOccupied = false;
        spawnedBall = null;

        float posX, posZ;
        MeshCollider bound = _attacker.GetComponent<Land>().ground.GetComponent<MeshCollider>();
        posX = Random.Range(bound.bounds.min.x, bound.bounds.max.x);
        posZ = Random.Range(bound.bounds.min.z, bound.bounds.max.z);
        Vector3 newPos = new Vector3(posX, 0.5f, posZ);
        spawnedBall = Instantiate(ball, newPos, Quaternion.identity);


        //Debug.Log($"boundMinX: {bound.bounds.min.x}, boundMaxX: {bound.bounds.max.x}");
        //Debug.Log($"boundMinZ: {bound.bounds.min.z}, boundMaxZ: {bound.bounds.max.z}");
        //Debug.Log($"posX: {posX}, posZ: {posZ}");
    }

    public void Play()
    {
        isAttackersTurn = true;
        gameState = GameState.PLAY;
        SetupPosition(landBottom, landTop);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

public enum GameState
{
    PLAY, MAIN_MENU, GAME_END
}

[System.Serializable]
public class Mode
{
    public float energy;
    public GameObject land;
    public WidgetPlayer ui;
}
