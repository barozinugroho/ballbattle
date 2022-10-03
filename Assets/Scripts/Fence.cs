using UnityEngine;

public class Fence : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Collision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collision(other.gameObject);
    }

    private void Collision(GameObject _other)
    {
        if (_other.CompareTag("Player"))
        {
            GameManager.instance.listAttackers.freeAttackers.Remove(_other.transform);
            if (transform.parent.CompareTag("Attacker") && !_other.GetComponent<Soldier>().param.isAttacker)
            {
                Destroy(_other);
            }

            if (transform.parent.CompareTag("Defender") && _other.GetComponent<Soldier>().param.isAttacker)
            {
                Destroy(_other);
            }
        }
    }
}
