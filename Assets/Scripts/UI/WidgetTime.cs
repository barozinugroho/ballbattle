using UnityEngine;
using UnityEngine.UI;

public class WidgetTime : MonoBehaviour
{
    public Text titleText;
    public Text timerText;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = GameManager.instance.timeLimits;
        titleText.text = $"Time Remaining";
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = $"{(int)timer} s";
    }
}
