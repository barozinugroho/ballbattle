using UnityEngine;

public class SoldierCollision : MonoBehaviour
{
    private Soldier soldier;

    private void Start()
    {
        soldier = GetComponent<Soldier>();
    }

    private void OnCollision(Collision _col, string _event)
    {
        switch (_col.gameObject.tag)
        {
            case "Player":
                Debug.Log($"Collision Player {_event}");
                break;
            case "Detector":
                Debug.Log($"Collision Detector {_event}");
                break;
            case "Ball":
                Debug.Log($"Collision Ball {_event}");
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision, "Enter");
    }

    private void OnCollisionStay(Collision collision)
    {
        OnCollision(collision, "Stay");
    }

    private void OnCollisionExit(Collision collision)
    {
        OnCollision(collision, "Exit");
    }
}
