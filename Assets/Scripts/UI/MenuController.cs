using UnityEngine;

public class MenuController : MonoBehaviour, IEventHook
{
    [SerializeField]
    private DataBindContext m_DataBindContext;
    [SerializeField]
    private string m_TwitterUrl;
    [SerializeField]
    private string m_FacebookUrl;

    public void OnInvoke(EventId eventId)
    {
        if (eventId == EventId.StartGame) {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        var gd = GameData.instance;

        m_DataBindContext["bestScore"] = gd.bestScore;
        m_DataBindContext["gamesPlayed"] = gd.gamesPlayed;
        m_DataBindContext["coins"] = gd.coins;
    }

    public void OnAchievementsClick()
    {
#if UNITY_ANDROID
        GooglePlayManager.Instance.ShowAchievementsUI();
#endif
    }

    public void OnLeaderboardsClick()
    {
#if UNITY_ANDROID
        GooglePlayManager.Instance.ShowLeaderBoard("leaderboard_high_scores");
#endif
    }

    public void OnTwitterClick()
    {
        Application.OpenURL(m_TwitterUrl);
    }

    public void OnFacebookClick()
    {
        Application.OpenURL(m_FacebookUrl);
    }
}