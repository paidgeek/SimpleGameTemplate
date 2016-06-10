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
                Debug.Log("ActionConnectionResultReceived: " + result.code);

                if (result.IsSuccess) {
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
#if UNITY_ANDROID
        GooglePlayManager.ActionLeaderboardsLoaded = lbResult =>
        {
            Debug.Log("ActionLeaderboardsLoaded: " + lbResult.Response);

            if (lbResult.IsSucceeded) {
                var bestScore = (int)GooglePlayManager.Instance.GetLeaderBoard("CgkI19aihIAQEAIQAA")
                                                       .GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME,
                                                           GPCollectionType.FRIENDS)
                                                       .LongScore;
                GameData.instance.bestScore = Mathf.Max(0, bestScore);
            }

            SceneManager.LoadScene("Main");
        };
        GooglePlayManager.Instance.LoadLeaderBoards();
#endif
    }
}