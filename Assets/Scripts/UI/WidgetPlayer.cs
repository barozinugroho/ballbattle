using UnityEngine;
using UnityEngine.UI;

public class WidgetPlayer : MonoBehaviour
{
    public UIMainMenu mainMenu;

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
            //Debug.Log($"timer: {timer}");
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
                //Debug.Log($"Regen");
                UpdateUI();
                //AddEnergy();
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
            i.fillAmount = 0f;
        }
    }

    public void ReduceEnergy(int _amount, int _current)
    {
        //Debug.Log($"reduce amount energy: {_amount} {_current}");
        int min = _current - _amount;
        if (min == 0)
        {
            for (int i = _amount; i >= min; i--)
            {
                if (bars[i].fillAmount == 1f)
                {
                    bars[i].fillAmount = 0f;
                }
            }
        }
        else
        {
            for (int i = _amount; i > min; i--)
            {
                if (bars[i].fillAmount == 1f)
                {
                    bars[i].fillAmount = 0f;
                }
            }
        }
    }

    private void UpdateUI()
    {
        if (bars[(int)Energy()].fillAmount < 1f)
        {
            bars[(int)Energy()].color = RegenColor();
            bars[(int)Energy()].fillAmount += EnergyRegen();
        }
        else
        {
            if (Energy() % 1f == 0f)
            {
                bars[(int)Energy()].color = HighlightColor();
                AddEnergy();
            }
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
                //GameManager.instance.attacker.energy += GameManager.instance.attackerParam.energyRegeneration;
                GameManager.instance.attacker.energy++;
                break;
            case GameManager.DEFENDER_TAG:
                //GameManager.instance.defender.energy += GameManager.instance.defenderParam.energyRegeneration;
                GameManager.instance.defender.energy++;
                break;
        }
        //Debug.Log($"energi: {Energy()} modulo: {Energy() % 1f}");
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
                return mainMenu.attackerColor;
            case GameManager.DEFENDER_TAG:
                return mainMenu.defenderColor;
            default:
                return Color.white;
        }
    }

    private Color RegenColor()
    {
        switch (tag)
        {
            case GameManager.ATTACKER_TAG:
                return mainMenu.attackerRegencolor;
            case GameManager.DEFENDER_TAG:
                return mainMenu.defenderRegencolor;
            default:
                return Color.white;
        }
    }
}
