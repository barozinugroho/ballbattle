using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject soldier;
    public GameObject landTop, landBottom;
    public GameObject ball;

    public Color attackerColor;
    public Color defenderColor;

    public int maxHP;
    public float energyRegeneration;
    public float timeLimits;

    public Mode attacker;
    public SoldierParam attackerParam;

    public Mode defender;
    public SoldierParam defenderParam;

    public ListAvailableAttacker listAttackers;

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

        listAttackers = GetComponent<ListAvailableAttacker>();
        listAttackers.freeAttackers = new List<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupLand();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, 1 << 6))
            {
                SpawnSoldier(hit.point, hit.collider);
                
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
            case "Attacker":
                go = Instantiate(soldier, _pos, Quaternion.LookRotation(landTop.transform.position));
                //go.tag = _col.tag;
                go.GetComponent<Soldier>().param = attackerParam;
                listAttackers.freeAttackers.Add(go.transform);
                break;
            case "Defender":
                go = Instantiate(soldier, _pos, Quaternion.LookRotation(landBottom.transform.position));
                //go.tag = _col.tag;
                go.GetComponent<Soldier>().param = defenderParam;
                break;
        }
    }

    private void SetupLand()
    {
        landTop.tag = "Defender";
        landTop.GetComponent<Land>().ChangeMat();

        landBottom.tag = "Attacker";
        landBottom.GetComponent<Land>().ChangeMat();

        attacker.hp = 0;
        attacker.land = landBottom;

        defender.hp = 0;
        defender.land = landTop;

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
    public int hp;
    public GameObject land;
    public GameObject ui;
}
