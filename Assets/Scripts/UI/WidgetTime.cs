using UnityEngine;
using UnityEngine.UI;

public class WidgetTime : MonoBehaviour
{
    public Text titleText;
    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = $"{(int)GameManager.instance.timer} s";
    }
}
