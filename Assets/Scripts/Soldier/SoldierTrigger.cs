using UnityEngine;

public class SoldierTrigger : MonoBehaviour
{
    private Soldier soldier;

    void Start()
    {
        soldier = GetComponent<Soldier>();
    }

    private void OnTrigger(Collider _other, string _event)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                //Debug.Log($"Trigger Player {_event}");
                if (soldier.param.isAttacker) // Attacker has ball meets defender
                {
                    if (!other.GetComponent<Soldier>().param.isAttacker)
                    {
                        if (transform.GetChild(0).CompareTag("Ball"))
                        {
                            // inactive
                            if (Vector3.Distance(transform.position, other.transform.position) <= 1f)
                            {
                                Debug.Log("Attacker inactive");
                                StartCoroutine(soldier.WaitInactive(soldier.param.reactivateTIme));

                                // find nearest attacker
                                soldier.FindNearestAttacker();

                                // pass the ball to nearest attackers
                                transform.GetChild(0).GetComponent<Ball>().player = soldier.LessDistance();
                                transform.GetChild(0).GetComponent<Ball>().speed = soldier.param.ballSpeed;
                                //transform.GetChild(0).parent = null;
                                GameManager.instance.isBallOccupied = false;
                            }
                        }
                    }
                }
                else // defender meets attacker has ball
                {
                    if (other.transform.GetChild(0).CompareTag("Ball"))
                    {
                        // inactive
                        if (Vector3.Distance(transform.position, other.transform.position) <= 1f)
                        {
                            Debug.Log("Defender inactive");
                            StartCoroutine(soldier.WaitInactive(soldier.param.reactivateTIme));
                        }
                    }
                }
                break;
            case "Detector":
                //Debug.Log($"Trigger Detector {_event}");
                if (soldier.param.isAttacker)
                {
                    if (soldier.isActive)
                    {
                        if (soldier.transform.GetChild(0).CompareTag("Ball"))
                        {
                            other.transform.parent.GetComponent<Soldier>().target = soldier.transform;
                        }
                    }
                }
                break;
            case "Ball":
                //Debug.Log($"Trigger Ball {_event}");
                if (soldier.param.isAttacker)
                {
                    if (soldier.isActive)
                    {
                        if (!GameManager.instance.isBallOccupied)
                        {
                            soldier.highlight.SetActive(true);

                            other.transform.SetParent(transform);
                            other.transform.SetAsFirstSibling();

                            GameManager.instance.isBallOccupied = true;
                            GameManager.instance.listSoldier.freeAttackers.Remove(transform);
                        }
                    }
                }
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //OnTrigger(other, "Stay");
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Detector":

                break;
        }
    }
}
