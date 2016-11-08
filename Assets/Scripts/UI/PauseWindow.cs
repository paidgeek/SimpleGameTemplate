using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseWindow : Singleton<PauseWindow>, IEventHook
{
  [SerializeField]
  private DataBindContext m_DataBindContext;

  public void OnInvoke(EventId eventId)
  {
    if (eventId == EventId.PauseGame) {
      Countdown.instance.CancelCountdown();
      gameObject.SetActive(true);
      m_DataBindContext["score"] = GameController.instance.score;
      m_DataBindContext["coins"] = GameController.instance.coins;

      foreach (var audioSource in FindObjectsOfType<AudioSource>()) {
        if (audioSource.loop) {
          audioSource.Pause();
        }
      }

      Time.timeScale = 0.0f;
    } else if (eventId == EventId.ContinueGame) {
      gameObject.SetActive(false);
      Countdown.instance.DoCountdown(3, () =>
      {
        Time.timeScale = 1.0f;
        foreach (var audioSource in FindObjectsOfType<AudioSource>()) {
          if (audioSource.loop) {
            audioSource.UnPause();
          }
        }
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
      gd.bestScore = score;
      SocialPlatform.ReportScore(score, success => {
        if (!success) {
          PlayerPrefs.SetInt("UnreportedScore", score);
          PlayerPrefs.Save();
        }
      });
      SocialPlatform.CheckAchievements(score);
    }

    gd.coins += coins;

    SceneManager.LoadScene(SceneManager.GetActiveScene()
      .buildIndex);
  }
}