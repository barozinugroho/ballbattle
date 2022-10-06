using UnityEngine;

public class SoldierCollision : MonoBehaviour
{
    private Soldier soldier;

    private void Start()
    {
        soldier = GetComponent<Soldier>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                //Debug.Log("isAttacker "+collision.transform.GetComponent<Soldier>().param.isAttacker);
                if (soldier.param.isAttacker) //Attacker has ball meet Defender
                {
                    if (!collision.transform.GetComponent<Soldier>().param.isAttacker)
                    {
                        if (transform.GetChild(0).CompareTag("Ball"))
                        {
                            StartCoroutine(soldier.WaitInactive(soldier.param.reactivateTIme));
                            soldier.FindNearestAttacker();
                            transform.GetChild(0).GetComponent<Ball>().player = soldier.LessDistance();
                            transform.GetChild(0).GetComponent<Ball>().speed = soldier.param.ballSpeed;
                            transform.GetChild(0).parent = null;
                            GameManager.instance.isBallOccupied = false;
                            //transform.GetChild(0).GetComponent<Ball>().SetParent();
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
                            soldier.target = null;
                            StartCoroutine(soldier.WaitInactive(soldier.param.reactivateTIme));
                            soldier.defenderReturn = true;
                        }
                    }
                }

                break;
            case "Ball":
                if (soldier.param.isAttacker)
                {
                    if (!GameManager.instance.isBallOccupied)
                    {
                        soldier.highlight.SetActive(true);

                        collision.transform.SetParent(transform);
                        collision.transform.SetAsFirstSibling();

                        GameManager.instance.isBallOccupied = true;
                        GameManager.instance.listSoldier.freeAttackers.Remove(transform);
                    }
                }
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (!soldier.param.isAttacker)
                {
                    if (collision.transform.GetComponent<Soldier>().param.isAttacker)
                    {
                        if (collision.transform.GetChild(0).CompareTag("Ball"))
                        {
                            soldier.target = null;
                            StartCoroutine(soldier.WaitInactive(soldier.param.reactivateTIme));
                            soldier.defenderReturn = true;
                        }
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
                        soldier.target = collision.gameObject.transform;
                    }
                }
                break;
        }
    }
}
