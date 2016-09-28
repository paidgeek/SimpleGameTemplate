using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
	private void Start()
	{
#if UNITY_ANDROID
		AdTapsy.StartSessionAndroid("5763ee89e4b03255c357c25f");
		var config = new PlayGamesClientConfiguration.Builder().Build();
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.Activate();
#endif

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

#if UNITY_ANDROID
        PlayGamesPlatform.Instance.LoadScores(GooglePlayIds.leaderboard_high_scores, LeaderboardStart.PlayerCentered, 1,
            LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, data =>
            {
                var onlineScore = 0;
                var prefsScore = GameData.instance.bestScore;

                if (data.Valid) {
                    onlineScore = (int) data.PlayerScore.value;
                }

                var bestScore = Mathf.Max(unreportedScore, Mathf.Max(onlineScore, prefsScore));
                GameData.instance.bestScore = bestScore;

                if (unreportedScore > onlineScore) {
                    Social.ReportScore(bestScore, GooglePlayIds.leaderboard_high_scores, success =>
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
#endif
	}
}