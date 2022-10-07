using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gameMenu;
    public GameObject winLoseScreen;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        winLoseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPlay()
    {
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
        GameManager.instance.Play();
    }

    public void ButtonQuit()
    {
        GameManager.instance.Quit();
    }
}
