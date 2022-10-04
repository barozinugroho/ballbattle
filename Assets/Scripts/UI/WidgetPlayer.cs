using UnityEngine;
using UnityEngine.UI;

public class WidgetPlayer : MonoBehaviour
{
    public Text nameText;
    public Image[] bars;

    // Start is called before the first frame update
    void Start()
    {
        EmptyBars();
    }

    int index = 0;
    // Update is called once per frame
    void Update()
    {
        if (index < GameManager.instance.maxHP)
        {
            UpdateUIEnergy(index);
        }
    }

    private void EmptyBars()
    {
        foreach (Image i in bars)
        {
            i.fillAmount = 0;
        }
    }

    public void UpdateWidget(string _mode)
    {
        nameText.text = _mode;
    }

    public void ResetUIEnergyBar()
    {
        index = 0;
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
            bars[i].fillAmount += GameManager.instance.energyRegeneration * Time.deltaTime;
        }
        else
        {
            bars[i].color = HighlightColor();
            index++;
        }
    }

    private Color HighlightColor()
    {
        switch (tag)
        {
            case "Attacker":
                return GameManager.instance.attackerColor;
            case "Defender":
                return GameManager.instance.defenderColor;
            default:
                return Color.white;
        }
    }

    private Color RegenColor()
    {
        switch (tag)
        {
            case "Attacker":
                return GameManager.instance.attackerRegencolor;
            case "Defender":
                return GameManager.instance.defenderRegencolor;
            default:
                return Color.white;
        }
    }
}
