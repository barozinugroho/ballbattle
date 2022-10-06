using UnityEngine;
using UnityEngine.UI;

public class WidgetPlayer : MonoBehaviour
{
    public Text nameText;
    public Image[] bars;

    // Start is called before the first frame update
    void Start()
    {
        ResetUIEnergyBar();
    }

    private float timeToRegen = 1f;
    private float timer = 0f;

    private bool IsTimeToRegen()
    {
        if (timer > timeToRegen)
        {
            timer = 0f;
            return true;
        }
        else
        {
            timer += Time.deltaTime;
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Energy() < GameManager.instance.maxHP)
        {
            if (IsTimeToRegen())
            {

            }
        }
    }

    public void UpdateName(string _mode)
    {
        nameText.text = _mode.ToUpper();
    }

    public void ResetUIEnergyBar()
    {
        foreach (Image i in bars)
        {
            i.fillAmount = 1f;
        }
    }

    private float Energy()
    {
        switch (tag)
        {
            case GameManager.ATTACKER_TAG:
                return GameManager.instance.attacker.energy;
            case GameManager.DEFENDER_TAG:
                return GameManager.instance.defender.energy;
            default:
                return 0;
        }
    }

    private void AddEnergy()
    {
        switch (tag)
        {
            case GameManager.ATTACKER_TAG:
                GameManager.instance.attacker.energy += GameManager.instance.attackerParam.energyRegeneration;
                break;
            case GameManager.DEFENDER_TAG:
                GameManager.instance.defender.energy += GameManager.instance.defenderParam.energyRegeneration;
                break;
        }
        Debug.Log($"energi: {Energy()} modulo: {Energy() % 1f}");
    }

    private float EnergyRegen()
    {
        switch (tag)
        {
            case GameManager.ATTACKER_TAG:
                return GameManager.instance.attackerParam.energyRegeneration;
            case GameManager.DEFENDER_TAG:
                return GameManager.instance.defenderParam.energyRegeneration;
            default:
                return 0f;
        }
    }

    private Color HighlightColor()
    {
        switch (tag)
        {
            case GameManager.ATTACKER_TAG:
                return GameManager.instance.attackerColor;
            case GameManager.DEFENDER_TAG:
                return GameManager.instance.defenderColor;
            default:
                return Color.white;
        }
    }

    private Color RegenColor()
    {
        switch (tag)
        {
            case GameManager.ATTACKER_TAG:
                return GameManager.instance.attackerRegencolor;
            case GameManager.DEFENDER_TAG:
                return GameManager.instance.defenderRegencolor;
            default:
                return Color.white;
        }
    }
}
