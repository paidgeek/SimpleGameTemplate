using UnityEngine;

public class InGameMenuController : MonoBehaviour, IEventHook
{
    [SerializeField]
    private DataBindContext m_DataBindContext;

    public void OnInvoke(string eventId)
    {
        if (eventId == "StartGame") {
            gameObject.SetActive(true);

            m_DataBindContext["score"] = 0;
            m_DataBindContext["coins"] = 0;
        } else if (eventId == "EndGame") {
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        var gc = GameController.instance;

        gc.onScoreChanged.AddListener(OnScoreChanged);
        gc.onCoinsChanged.AddListener(OnCoinsChanged);
    }

    private void OnCoinsChanged(int coins)
    {
        m_DataBindContext["coins"] = coins;
    }

    private void OnScoreChanged(int score)
    {
        m_DataBindContext["score"] = score;
    }

    public void OnPauseClick() {}
}