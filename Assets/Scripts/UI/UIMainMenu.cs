using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public static UIMainMenu instance;

    public GameObject mainMenu;
    public GameObject gameMenu;
    public GameObject winLoseScreen;

    public Text topText, bottomText;

    public Color attackerColor;
    public Color attackerRegencolor;

    public Color defenderColor;
    public Color defenderRegencolor;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        winLoseScreen.SetActive(false);
    }

    public void GameEnds(bool _isAttackerWin)
    {
        if (GameManager.instance.attacker.land.name == "LandTop")
        {
            topText.text = "WIN";
            bottomText.text = "LOSE";
        }
        else if (GameManager.instance.attacker.land.name == "LandBottom")
        {
            topText.text = "LOSE";
            bottomText.text = "WIN";
        }
        
        winLoseScreen.SetActive(true);
        mainMenu.SetActive(false);
        gameMenu.SetActive(false);
    }

    public void ButtonPlay()
    {
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
        GameManager.instance.Play();
    }

    public void ButtonBackToMainMenu()
    {
        mainMenu.SetActive(true);
        winLoseScreen.SetActive(false);
        gameMenu.SetActive(false);
    }

    public void ButtonQuit()
    {
        GameManager.instance.Quit();
    }
}
