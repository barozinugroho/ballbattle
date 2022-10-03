using UnityEngine;

public class Fence : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (transform.parent.CompareTag("Attacker") && !collision.gameObject.GetComponent<Soldier>().param.isAttacker)
            {
                Destroy(collision.gameObject);
            }

            if (transform.parent.CompareTag("Defender") && collision.gameObject.GetComponent<Soldier>().param.isAttacker)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
