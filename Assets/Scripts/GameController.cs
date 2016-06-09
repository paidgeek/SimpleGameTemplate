using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField]
    private EventDispatcher m_EventDispatcher;

    public void StartGame()
    {
        m_EventDispatcher.Invoke("startGame");
    }

    public void EndGame()
    {
        m_EventDispatcher.Invoke("endGame");
    }
}
