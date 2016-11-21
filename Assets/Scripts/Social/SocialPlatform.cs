using UnityEngine;
using UnityEngine.Events;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;

#endif

public static class SocialPlatform
{
  public static void Activate()
  {
#if UNITY_ANDROID
    var config = new PlayGamesClientConfiguration.Builder().Build();
    PlayGamesPlatform.InitializeInstance(config);
    PlayGamesPlatform.Activate();
#endif
  }

  public static void SignOut()
  {
#if UNITY_ANDROID
    PlayGamesPlatform.Instance.SignOut();
#endif
  }

  public static void LoadMyScore(UnityAction<int> callback)
  {
#if UNITY_ANDROID
    PlayGamesPlatform.Instance.LoadScores(GooglePlayIds.leaderboard_high_scores, LeaderboardStart.PlayerCentered, 1,
      LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, data =>
      {
        var score = 0;
        if (data.Valid) {
          score = (int) data.PlayerScore.value;
        }
        callback(score);
      });
#endif
  }

  public static void ReportScore(int score, UnityAction<bool> callback)
  {
    Social.ReportScore(score, GooglePlayIds.leaderboard_high_scores, success =>
    {
      callback(success);
    });
  }

  public static void CompleteAchievement(string achievementId, UnityAction<bool> callback = null)
  {
#if UNITY_ANDROID
    PlayGamesPlatform.Instance.ReportProgress(achievementId, 100.0, success =>
    {
      if (callback != null) {
        callback(success);
      }
    });
#endif
  }

  public static void ShowLeaderboard(string leaderboardId)
  {
#if UNITY_ANDROID
    PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
#endif
  }

  public static void ShowAchievements()
  {
    Social.ShowAchievementsUI();
  }

  public static void CheckAchievements(int score)
  {
    UnityAction<bool> callback = success =>
    {
      if (!success) {
        PlayerPrefs.SetInt("ReportAchievementFailed", 1);
        PlayerPrefs.Save();
      }
    };

    if (!PlayGamesPlatform.Instance.IsAuthenticated()) {
      callback(false);
    }
  }
}