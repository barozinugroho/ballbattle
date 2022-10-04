using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private IEnumerator WaitSpawnBall()
    {
        yield return new WaitForSecondsRealtime(3f);
        GameManager.instance.SpawnBall(GameManager.instance.attacker.land);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (collision.transform.parent.GetComponent<Soldier>().param.isAttacker)
            {
                Destroy(collision.transform.parent.gameObject);
                GameManager.instance.SwitchMode();
            }

            /*if (transform.parent.CompareTag("Defender"))
            {
                StartCoroutine(WaitSpawnBall());
            }*/

            
            
        }
    }
}
