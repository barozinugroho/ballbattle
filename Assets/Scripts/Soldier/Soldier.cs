using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public SoldierParam param;

    public GameObject detectorArea;
    public GameObject highlight;
    public GameObject indicator;

    public Material attackerMat;
    public Material defenderMat;
    public Material inactiveMat;

    public Transform target;

    private new MeshRenderer renderer;
    private Rigidbody rigidbody;
    private CapsuleCollider collider;

    private Vector3 originPos;
    private Land land;

    public bool defenderReturn;
    public bool isActive;
    public bool isMove;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        land = GameManager.instance.defender.land.GetComponent<Land>();
        originPos = transform.position;
        StartCoroutine(WaitInactive(param.spawnTime));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActive)
        {
            if (param.isAttacker)
            {
                if (!GameManager.instance.isBallOccupied)
                {
                    RotateSoldier(GameManager.instance.spawnedBall.transform);
                }
                else
                {
                    if (transform.GetChild(0).CompareTag("Ball"))
                    {
                        RotateSoldier(land.gate.transform);
                    }
                    else
                    {
                        Vector3 direction = land.fence.transform.position - transform.position;
                        direction.y = 0f;
                        direction.x = 0f;
                        Vector3 rotateToTarget = Vector3.RotateTowards(transform.forward, direction, 3f * Time.deltaTime, 0.0f);
                        transform.rotation = Quaternion.LookRotation(rotateToTarget);

                        //collider.isTrigger = true;
                    }
                }

                if (transform.GetChild(0).CompareTag("Ball"))
                {
                    //move with ball
                    transform.Translate(Vector3.forward * param.carryingSpeed * Time.deltaTime);
                    isMove = true;
                }
                else
                {
                    //move without ball
                    transform.Translate(Vector3.forward * param.normalSpeed * Time.deltaTime);
                    isMove = true;
                }
            }
            else //Defender
            {
                //lock target
                if (target)
                {
                    RotateSoldier(target);

                    transform.Translate(Vector3.forward * param.normalSpeed * Time.deltaTime);
                    isMove = true;

                    detectorArea.SetActive(false);
                }
            }
            
        }
        else
        {
            if (!param.isAttacker && defenderReturn)
            {
                transform.position = originPos;
                /*// inactive defender return to origin position
                Vector3 direction = originPos - transform.position;
                direction.y = 0f;
                Vector3 rotateToTarget = Vector3.RotateTowards(transform.forward, direction, 3f * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(rotateToTarget);

                if (transform.position != originPos)
                {
                    transform.Translate(Vector3.forward * param.returnSpeed * Time.deltaTime);
                    isMove = true;
                    //defenderReturn = false;
                }*/

            }
        }

        indicator.SetActive(isMove);

        if (param.isAttacker)
        {
            if (GameManager.instance.isBallOccupied && !transform.GetChild(0).CompareTag("Ball"))
            {
                rigidbody.isKinematic = true;
            }
            else
            {
                rigidbody.isKinematic = false;
            }
        }
    }

    private void RotateSoldier(Transform _target)
    {
        Vector3 direction = _target.position - transform.position;
        direction.y = 0f;
        //direction.x = 0f;
        Vector3 rotateToTarget = Vector3.RotateTowards(transform.forward, direction, 3f * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(rotateToTarget);
    }

    public IEnumerator WaitInactive(float _time)
    {
        //collider.isTrigger = true;
        rigidbody.isKinematic = true;
        isActive = false;
        isMove = false;
        renderer.material = inactiveMat;
        detectorArea.SetActive(false);
        highlight.SetActive(false);
        indicator.SetActive(false);
        yield return new WaitForSecondsRealtime(_time);
        isActive = true;
        OnSpawn();
    }

    private void OnSpawn()
    {
        if (param.isAttacker)
        {
            renderer.material = attackerMat;
            if (!GameManager.instance.isBallOccupied)
            {
                rigidbody.isKinematic = false;
            }
        }
        else
        {
            rigidbody.isKinematic = false;
            renderer.material = defenderMat;
            detectorArea.transform.localScale = new Vector3(param.detectionRange/10f, param.detectionRange/10f, 1f);
            detectorArea.SetActive(true);
        }
    }

    public void FindNearestAttacker()
    {
        //TODO: Find Nearest attackers
        GameManager.instance.listSoldier.attackersDistance = new List<SoldierNearest>();
        if (transform.GetChild(0).CompareTag("Ball"))
        {
            for (int i = 0; i < GameManager.instance.listSoldier.freeAttackers.Count; i++)
            {
                float temp = Vector3.Distance(transform.position, GameManager.instance.listSoldier.freeAttackers[i].position);
                SoldierNearest soldier = new SoldierNearest{ 
                    soldier = GameManager.instance.listSoldier.freeAttackers[i],
                    distance = temp
                        };
                GameManager.instance.listSoldier.attackersDistance.Add(soldier);
            }
        }
    }

    public Transform LessDistance()
    {
        SoldierNearest temp = null;
        if (GameManager.instance.listSoldier.attackersDistance.Count > 1)
        {
            for (int i = 0; i < GameManager.instance.listSoldier.attackersDistance.Count; i++)
            {
                if (i == 0)
                {
                    if (GameManager.instance.listSoldier.attackersDistance[i].distance < GameManager.instance.listSoldier.attackersDistance[i+1].distance)
                    {
                        temp = GameManager.instance.listSoldier.attackersDistance[i];
                    }
                    else
                    {
                        temp = GameManager.instance.listSoldier.attackersDistance[i + 1];
                    }
                }
                else
                {
                    if (GameManager.instance.listSoldier.attackersDistance[i].distance < temp.distance)
                    {
                        temp = GameManager.instance.listSoldier.attackersDistance[i];
                    }
                }
            }
            return temp.soldier;
        }
        else if (GameManager.instance.listSoldier.attackersDistance.Count == 1)
        {
            return GameManager.instance.listSoldier.attackersDistance[0].soldier;
        }
        else { return null; }

    }
}
