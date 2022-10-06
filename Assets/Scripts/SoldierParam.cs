using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoldierParam", order = 1)]
public class SoldierParam : ScriptableObject
{
    public bool isAttacker;
    public float energyRegeneration;
    public int energyCost;
    public float spawnTime;
    public float reactivateTIme;
    public float normalSpeed;
    public float carryingSpeed;
    public float ballSpeed;
    public float returnSpeed;
    public float detectionRange;
}