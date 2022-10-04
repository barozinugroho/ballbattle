using System.Collections;
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

    private new MeshRenderer renderer;
    private Rigidbody rigidbody;
    [SerializeField]private Transform target;
    private Vector3 originPos;
    private Land land;
    public bool isActive;
    public bool isMove;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        rigidbody = GetComponent<Rigidbody>();
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
                        Vector3 direction = land.gate.transform.position - transform.position;
                        direction.y = 0f;
                        Vector3 rotateToTarget = Vector3.RotateTowards(transform.forward, direction, 3f * Time.deltaTime, 0.0f);
                        transform.rotation = Quaternion.LookRotation(rotateToTarget);
                    }
                    else
                    {
                        Vector3 direction = land.fence.transform.position - transform.position;
                        direction.y = 0f;
                        direction.x = 0f;
                        Vector3 rotateToTarget = Vector3.RotateTowards(transform.forward, direction, 3f * Time.deltaTime, 0.0f);
                        transform.rotation = Quaternion.LookRotation(rotateToTarget);
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
                    Vector3 direction = target.position - transform.position;
                    direction.y = 0f;
                    Vector3 rotateToTarget = Vector3.RotateTowards(transform.forward, direction, 3f * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(rotateToTarget);

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

    private bool defenderReturn;

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Debug.Log("isAttacker "+collision.transform.GetComponent<Soldier>().param.isAttacker);
                if (param.isAttacker) //Attacker has ball meet Defender
                {
                    if (!collision.transform.GetComponent<Soldier>().param.isAttacker)
                    {
                        if (transform.GetChild(0).CompareTag("Ball"))
                        {
                            StartCoroutine(WaitInactive(param.reactivateTIme));
                            //transform.GetChild(0).parent = null;
                        }
                    }
                }
                else //Defender meet attacker that has ball
                {
                    if (collision.transform.GetComponent<Soldier>().param.isAttacker)
                    {
                        if (collision.transform.GetChild(0).CompareTag("Ball"))
                        {
                            target = null;
                            StartCoroutine(WaitInactive(param.reactivateTIme));
                            defenderReturn = true;
                        }
                    }
                }
                
                break;
            case "Ball":
                if (param.isAttacker)
                {
                    if (!GameManager.instance.isBallOccupied)
                    {
                        highlight.SetActive(true);

                        collision.transform.SetParent(transform);
                        collision.transform.SetAsFirstSibling();
                        
                        GameManager.instance.isBallOccupied = true;
                        GameManager.instance.listAttackers.freeAttackers.Remove(transform);
                    }
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ball":
                break;
            case "Player":
                if (collision.GetComponent<Soldier>().param.isAttacker)
                {
                    if (collision.transform.GetChild(0).gameObject.CompareTag("Ball"))
                    {
                        //rigidbody.isKinematic = false;
                        target = collision.gameObject.transform;
                    }
                }
                break;
        }
    }

    private IEnumerator WaitInactive(float _time)
    {
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

    private void FindNearestAttacker()
    {
        //TODO: Find Nearest attackers
    }
}
