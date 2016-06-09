using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour, IEventHook
{
    [SerializeField]
    private DataBindContext m_DataBindContext;

    public void OnInvoke(string eventId)
    {
        if (eventId == "EndGame") {
            gameObject.SetActive(true);

            var gc = GameController.instance;
            var gd = GameData.instance;

            m_DataBindContext["lastScore"] = gc.score;
            m_DataBindContext["lastCoins"] = gc.coins;
            m_DataBindContext["bestScore"] = gd.bestScore;
            m_DataBindContext["coins"] = gd.coins;
        }
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene()
                                           .buildIndex);
    }

    public void OnRetryClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene()
                                           .buildIndex);
        PlayerPrefs.SetInt("IsRetry", 1);
        PlayerPrefs.Save();
    }

    public void OnShareClick() {}
}