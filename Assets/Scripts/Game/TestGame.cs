using UnityEngine;

public class TestGame : MonoBehaviour
{
    public void AddScore()
    {
        GameController.instance.score += 1;
    }

    public void AddCoins()
    {
        GameController.instance.coins += 100;
    }

    public void EndGame()
    {
        GameController.instance.EndGame();
    }
}