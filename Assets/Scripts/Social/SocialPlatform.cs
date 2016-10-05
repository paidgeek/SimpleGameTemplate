using System;
using UnityEngine;
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

  public static void LoadMyScore(Action<int> callback)
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

  public static void ReportScore(int score, Action<bool> callback)
  {
    Social.ReportScore(score, GooglePlayIds.leaderboard_high_scores, success =>
    {
      callback(success);
    });
  }
}