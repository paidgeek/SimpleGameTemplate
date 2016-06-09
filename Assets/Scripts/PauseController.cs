using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : Singleton<PauseController>, IEventHook
{
    [SerializeField]
    private DataBindContext m_DataBindContext;

    public void OnInvoke(EventId eventId)
    {
        if (eventId == EventId.PauseGame) {
            gameObject.SetActive(true);
            m_DataBindContext["score"] = GameController.instance.score;
            m_DataBindContext["coins"] = GameController.instance.coins;
            Time.timeScale = 0.0f;
        } else if (eventId == EventId.ContinueGame) {
            gameObject.SetActive(false);
            CountdownController.instance.Countdown(3, () =>
            {
                Time.timeScale = 1.0f;
            });
        }
    }

    public void OnHomeClick()
    {
        Time.timeScale = 1.0f;

        var gd = GameData.instance;
        var score = GameController.instance.score;
        var coins = GameController.instance.coins;

        if (score > gd.bestScore) {
            GameController.instance.ReportScore();
            gd.bestScore = score;
        }

        gd.coins += coins;

        SceneManager.LoadScene(SceneManager.GetActiveScene()
                                           .buildIndex);
    }
}