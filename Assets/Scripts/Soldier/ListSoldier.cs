using System.Collections.Generic;
using UnityEngine;

public class ListSoldier : MonoBehaviour
{
    public List<Transform> freeAttackers = new List<Transform>();
    public List<Transform> freeDefenders = new List<Transform>();

    public List<SoldierNearest> attackersDistance = new List<SoldierNearest>();
}

[System.Serializable]
public class SoldierNearest
{
    public Transform soldier;
    public float distance;
}
