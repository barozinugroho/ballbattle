using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public int maxMatch;
    public int matchCounter;
    public int attackersScore;
    public int defendersSCore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMatchResult(string _status)
    {
        if (matchCounter < maxMatch - 1)
        {
            matchCounter++;
            switch (_status)
            {
                case "win":
                    attackersScore++;
                    break;
                case "lose":
                    defendersSCore++;
                    break;
                case "draw":
                    break;
            }
        }
        else
        {
            UIMainMenu.instance.GameEnds(IsAttackerWin());
            GameManager.instance.gameState = GameState.GAME_END;
        }
    }

    private bool IsAttackerWin()
    {
        return attackersScore >= defendersSCore;
    }
}
