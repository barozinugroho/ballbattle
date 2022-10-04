using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject soldier;
    public GameObject landTop, landBottom;
    public GameObject ball;

    public Color attackerColor;
    public Color attackerRegencolor;

    public Color defenderColor;
    public Color defenderRegencolor;

    public int maxHP;
    public float timeLimits;
    public float timer { get; private set; }

    public Mode attacker;
    public SoldierParam attackerParam;

    public Mode defender;
    public SoldierParam defenderParam;

    public ListSoldier listAttackers;

    public const string ATTACKER_TAG = "Attacker";
    public const string DEFENDER_TAG = "Defender";

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

        listAttackers = GetComponent<ListSoldier>();
        listAttackers.freeAttackers = new List<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isAttackersTurn = true;
        SetupPosition(landBottom, landTop);
    }

    // Update is called once per frame
    void Update()
    {
        OnTap();
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
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
                        if (isAttackersTurn && attacker.energy >= attackerParam.energyCost)
                        {
                            SpawnSoldier(hit.point, hit.collider);
                            isAttackersTurn = false;
                        }
                        break;
                    case DEFENDER_TAG:
                        if (!isAttackersTurn && defender.energy >= defenderParam.energyCost)
                        {
                            SpawnSoldier(hit.point, hit.collider);
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
                go = Instantiate(soldier, _pos, Quaternion.LookRotation(attacker.land.transform.position));
                go.GetComponent<Soldier>().param = attackerParam;
                listAttackers.freeAttackers.Add(go.transform);
                break;
            case DEFENDER_TAG:
                defender.energy -= defenderParam.energyCost;
                go = Instantiate(soldier, _pos, Quaternion.LookRotation(defender.land.transform.position));
                go.GetComponent<Soldier>().param = defenderParam;
                break;
        }
    }

    public void SwitchMode()
    {
        isAttackersTurn = true;
        Mode temp = attacker;
        attacker = defender;
        defender = temp;

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
        attacker.ui.UpdateWidget(ATTACKER_TAG);
        attacker.ui.tag = ATTACKER_TAG;

        defender.energy = 0;
        defender.land = _defenderPos;
        defender.ui = defender.land.GetComponent<Land>().uiPlayer;
        defender.ui.ResetUIEnergyBar();
        defender.ui.UpdateWidget(DEFENDER_TAG);
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
}

[System.Serializable]
public class Mode
{
    public int energy;
    public GameObject land;
    public WidgetPlayer ui;
}
