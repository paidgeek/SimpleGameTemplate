using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
  private void Start()
  {
    SocialPlatform.Activate();

    Social.localUser.Authenticate(success =>
    {
      if (success) {
        LoadScore();
      } else {
        SceneManager.LoadScene("Main");
      }
    });
  }

  private void LoadScore()
  {
    var unreportedScore = 0;

    if (PlayerPrefs.HasKey("UnreportedScore")) {
      unreportedScore = PlayerPrefs.GetInt("UnreportedScore");
      PlayerPrefs.DeleteKey("UnreportedScore");
      PlayerPrefs.Save();
    }

    SocialPlatform.LoadMyScore(score =>
    {
      var prefsScore = GameData.instance.bestScore;

      var bestScore = Mathf.Max(unreportedScore, Mathf.Max(score, prefsScore));
      GameData.instance.bestScore = bestScore;

      if (unreportedScore > score) {
        SocialPlatform.ReportScore(bestScore, success =>
        {
          if (!success) {
            PlayerPrefs.SetInt("UnreportedScore", bestScore);
            PlayerPrefs.Save();
          }

          SceneManager.LoadScene("Main");
        });
      } else {
        SceneManager.LoadScene("Main");
      }
    });
  }
}