using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (transform.parent.CompareTag("Defender"))
            {
                if (collision.transform.parent.GetComponent<Soldier>().param.isAttacker)
                {
                    Destroy(collision.transform.parent.gameObject);
                    GameManager.instance.SwitchMode();
                    GameManager.instance.match.UpdateMatchResult("win");
                }
            }            
        }
    }
}
