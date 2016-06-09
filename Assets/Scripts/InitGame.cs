using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
    private void Start()
    {
#if UNITY_ANDROID
        if (GooglePlayConnection.Instance.IsConnected) {
            GooglePlayManager.ActionLeaderboardsLoaded = lbResult =>
            {
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
        } else {
            GooglePlayConnection.ActionConnectionResultReceived = result =>
            {
                Debug.Log("ActionConnectionResultReceived: " + result.code);

                if (result.IsSuccess) {
                    if (PlayerPrefs.HasKey("UnreportedScore")) {
                        var score = long.Parse(PlayerPrefs.GetString("UnreportedScore"));

                        GooglePlayManager.ActionScoreSubmited = scoreResult =>
                        {
                            if (scoreResult.IsSucceeded) {
                                PlayerPrefs.DeleteKey("UnreportedScore");
                                PlayerPrefs.Save();
                            }

                            SceneManager.LoadScene("Main");
                        };
                    } else {
                        GooglePlayManager.ActionLeaderboardsLoaded = lbResult =>
                        {
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
                    }
                } else {
                    SceneManager.LoadScene("Main");
                }
            };
            GooglePlayConnection.Instance.Connect();
        }
#endif
    }
}