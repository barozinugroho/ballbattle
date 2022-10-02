using System.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;

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
                    //transform.LookAt(GameManager.instance.spawnedBall.transform);
                    transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(GameManager.instance.spawnedBall.transform.position));
                }
                else
                {
                    if (transform.GetChild(0).CompareTag("Ball"))
                    {
                        transform.LookAt(GameManager.instance.defender.land.GetComponent<Land>().gate.transform);
                    }
                    else
                    {
                        transform.LookAt(GameManager.instance.defender.land.transform);
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
