using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour, IEventHook
{
    [SerializeField]
    private DataBindContext m_DataBindContext;
    [SerializeField]
    private RectTransform m_ShareScoreView;

    public void OnInvoke(EventId eventId)
    {
        if (eventId == EventId.EndGame) {
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

    public void OnShareClick()
    {
        StartCoroutine(ShareCoroutine());
    }

    private IEnumerator ShareCoroutine()
    {
        m_ShareScoreView.gameObject.SetActive(true);

        m_DataBindContext["lastScore"] = GameController.instance.score;
        m_DataBindContext["lastCoins"] = GameController.instance.coins;
        m_DataBindContext["bestScore"] = GameData.instance.bestScore;

        yield return new WaitForEndOfFrame();

        var width = (int) m_ShareScoreView.sizeDelta.x;
        var height = (int) m_ShareScoreView.sizeDelta.y;
        var texture = new Texture2D(width, height, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        var message = "OMG! I scored " + GameController.instance.score.ToString("N0") +
                      " in #SimpleGameTemplate! Can you beat my score? https://www.google.com";
        AndroidSocialGate.StartShareIntent("SimpleGameTemplate", message, texture);

        m_ShareScoreView.gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();
    }
}