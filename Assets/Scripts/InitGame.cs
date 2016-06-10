using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
    private void Start()
    {
#if UNITY_ANDROID
        if (GooglePlayConnection.Instance.IsConnected) {
            LoadScore();
        } else {
            GooglePlayConnection.ActionConnectionResultReceived = result =>
            {
                if (result.IsSuccess) {
                    LoadScore();
                    /*
                    if (PlayerPrefs.HasKey("UnreportedScore")) {
                        var score = PlayerPrefs.GetInt("UnreportedScore");

                        GooglePlayManager.ActionScoreSubmited = scoreResult =>
                        {
                            Debug.Log("ActionScoreSubmited: " + scoreResult.Response);

                            if (scoreResult.IsSucceeded) {
                                PlayerPrefs.DeleteKey("UnreportedScore");
                                PlayerPrefs.Save();
                            }

                            SceneManager.LoadScene("Main");
                        };
                        GooglePlayManager.Instance.SubmitScore("leaderboard_high_scores", score);
                    } else {
                        LoadScore();
                    }
                    */
                } else {
                    SceneManager.LoadScene("Main");
                }
            };
            GooglePlayConnection.Instance.Connect();
        }
#endif
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
        GooglePlayManager.ActionLeaderboardsLoaded = lbResult =>
        {
            var onlineScore = 0;
            var prefsScore = GameData.instance.bestScore;

            if (lbResult.IsSucceeded) {
                onlineScore = Mathf.Max(0, (int) GooglePlayManager.Instance.GetLeaderBoard("CgkI19aihIAQEAIQAA")
                                                                  .GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME,
                                                                      GPCollectionType.GLOBAL)
                                                                  .LongScore);
                Debug.Log("CgkI19aihIAQEAIQAA: " + onlineScore);
            }

            var bestScore = Mathf.Max(unreportedScore, Mathf.Max(onlineScore, prefsScore));
            GameData.instance.bestScore = bestScore;

            if (unreportedScore > onlineScore) {
                GooglePlayManager.ActionScoreSubmited = scoreResult =>
                {
                    if (!scoreResult.IsSucceeded) {
                        PlayerPrefs.SetInt("UnreportedScore", bestScore);
                        PlayerPrefs.Save();
                    }

                    SceneManager.LoadScene("Main");
                };
                GooglePlayManager.Instance.SubmitScore("leaderboard_high_scores", bestScore);
            } else {
                SceneManager.LoadScene("Main");
            }
        };
        GooglePlayManager.Instance.LoadLeaderBoards();
#endif
    }
}