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

    // Update is called once per frame
    void Update()
    {
        if (Energy() < GameManager.instance.maxHP)
        {
            UpdateUIEnergy(Energy());
        }
    }

    public void UpdateWidget(string _mode)
    {
        nameText.text = _mode;
    }

    public void ResetUIEnergyBar()
    {
        foreach (Image i in bars)
        {
            i.fillAmount = 0f;
        }
    }

    private void UpdateUIEnergy(int i)
    {
        if (bars[i].fillAmount < 1f)
        {
            bars[i].color = RegenColor();
            bars[i].fillAmount += EnergyRegen() * Time.deltaTime;
        }
        else
        {
            bars[i].color = HighlightColor();
            AddEnergy();
            //TODO: update UI energy based on cost energy
        }
    }

    private int Energy()
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
                GameManager.instance.attacker.energy++;
                break;
            case GameManager.DEFENDER_TAG:
                GameManager.instance.defender.energy++;
                break;
        }
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
