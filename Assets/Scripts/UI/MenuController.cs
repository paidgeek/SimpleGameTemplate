using UnityEngine;

public class MenuController : MonoBehaviour, IEventHook
{
    [SerializeField]
    private DataBindContext m_DataBindContext;
    [SerializeField]
    private string m_TwitterUserId;
    [SerializeField]
    private string m_FacebookPageId;

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

        Ads.instance.ShowBanner();
    }

    private void OnDisable()
    {
        Ads.instance.HideBanner();
    }

    public void OnAchievementsClick()
    {
        Social.ShowAchievementsUI();
    }

    public void OnLeaderboardsClick()
    {
        Social.ShowLeaderboardUI();
    }

    public void OnTwitterClick()
    {
        if (Util.IsAppInstalled("com.twitter.android")) {
            Application.OpenURL("twitter://user?user_id=" + m_TwitterUserId);
        } else {
            Application.OpenURL("https://twitter.com/intent/user?user_id=" + m_TwitterUserId);
        }
    }

    public void OnFacebookClick()
    {
        if (Util.IsAppInstalled("com.facebook.katana")) {
            Application.OpenURL("fb://page/" + m_FacebookPageId);
        } else {
            Application.OpenURL("https://www.facebook.com/" + m_FacebookPageId);
        }
    }
}