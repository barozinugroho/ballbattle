using UnityEngine;
using UnityEngine.UI;

public class WidgetPlayer : MonoBehaviour
{
    public Text nameText;
    public Image[] bars;

    public Color regen;

    // Start is called before the first frame update
    void Start()
    {
        regen = GameManager.instance.attackerColor;
        EmptyBars();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateUIEnergy();
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

    private void UpdateUIEnergy(int i)
    {
        if (bars[i].fillAmount < 1f)
        {
            bars[i].color = regen;
            bars[i].fillAmount += GameManager.instance.energyRegeneration * Time.deltaTime;
        }
        else
        {
            bars[i].color = GameManager.instance.attackerColor;
        }
    }
}
