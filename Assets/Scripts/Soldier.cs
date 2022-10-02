using System.Collections;
using System.Numerics;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Soldier : MonoBehaviour
{
    public SoldierParam param;

    public GameObject detectorArea;

    public Material attackerMat;
    public Material defenderMat;
    public Material inactiveMat;

    private new MeshRenderer renderer;
    private CapsuleCollider capsuleCollider;
    public bool isActive;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        detectorArea.SetActive(false);
        StartCoroutine(WaitInactive(param.spawnTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (param.isAttacker)
            {
                if (!GameManager.instance.isBallOccupied)
                {
                    //find ball
                    /*Vector3 direction = GameManager.instance.spawnedBall.transform.position - transform.position;
                    direction.y = 0f;
                    Quaternion rotateTOTarget = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotateTOTarget, 3f*Time.deltaTime);*/

                    Vector3 direction = GameManager.instance.spawnedBall.transform.position - transform.position;
                    direction.y = 0f;
                    Vector3 rotateToTarget = Vector3.RotateTowards(transform.forward, direction, 3f * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(rotateToTarget);
                }
                else
                {
                    if (transform.GetChild(0).CompareTag("Ball"))
                    {
                        Vector3 direction = new Vector3(GameManager.instance.defender.land.GetComponent<Land>().gate.transform.position.x,
                            this.transform.position.y,
                            GameManager.instance.defender.land.GetComponent<Land>().gate.transform.position.z);
                        transform.LookAt(direction);
                    }
                    else
                    {
                        Vector3 direction = new Vector3(GameManager.instance.defender.land.GetComponent<Land>().fence.transform.position.x,
                            this.transform.position.y,
                            GameManager.instance.defender.land.GetComponent<Land>().fence.transform.position.z);
                        transform.LookAt(direction);
                    }
                }
            }

            if (transform.GetChild(0).CompareTag("Ball"))
            {
                //move with ball
                transform.Translate(Vector3.forward * param.carryingSpeed * Time.deltaTime);
            }
            else
            {
                //move without ball
                transform.Translate(Vector3.forward * param.normalSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Fence":
                isActive = false;
                Destroy(gameObject);
                break;
            case "Gate":
                isActive = false;
                Destroy(gameObject);
                break;
            case "Ball":
                if (!param.isAttacker)
                {
                    if (GameManager.instance.isBallOccupied)
                    {
                        isActive = false;
                    }
                }
                if (param.isAttacker)
                {
                    collision.transform.SetParent(transform);
                    collision.transform.SetAsFirstSibling();
                    GameManager.instance.isBallOccupied = true;
                    capsuleCollider.radius = 1.5f;
                }
                break;
            case "Player":
                if (!collision.GetComponent<Soldier>().param.isAttacker)
                {
                    if (transform.GetChild(0).CompareTag("Ball"))
                    {
                        isActive = false;
                    }
                }
                break;
        }
    }

    private IEnumerator WaitInactive(float _time)
    {
        isActive = false;
        renderer.material = inactiveMat;
        yield return new WaitForSecondsRealtime(_time);
        isActive = true;
        OnSpawn();
    }

    private void OnSpawn()
    {
        if (param.isAttacker)
        {
            renderer.material = attackerMat;
        }
        else
        {
            renderer.material = defenderMat;
            detectorArea.SetActive(true);
        }
    }
}
