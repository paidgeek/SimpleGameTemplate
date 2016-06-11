using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class GiftButton : MonoBehaviour
{
    [SerializeField]
    private DataBindContext m_DataBindContext;
    [SerializeField]
    private GameObject m_Button;
    private int m_Reward;

    private void OnEnable()
    {
        m_Button.SetActive(false);
        //StartCoroutine(LoadAdCoroutine());
    }

    public void OnClick()
    {
        /*
        if (!Advertisement.IsReady("rewardedVideoZone") && !Chartboost.hasRewardedVideo(CBLocation.Default)) {
            return;
        }

        m_Button.SetActive(false);
        var unityReady = Advertisement.IsReady("rewardedVideoZone");
        var cbReady = Chartboost.hasRewardedVideo(CBLocation.Default);

        if (unityReady && cbReady) {
            if (Random.Range(0, 2) == 0) {
                ShowUnityAd();
            } else {
                ShowChartboostAd();
            }
        } else if (unityReady) {
            ShowUnityAd();
        } else {
            ShowChartboostAd();
        }
        */
    }

    private void OnCompleteVideo()
    {
        GameData.instance.coins += m_Reward;
        m_DataBindContext["coins"] = GameData.instance.coins;
    }

    /*
    private IEnumerator LoadAdCoroutine()
    {
        while (!Advertisement.IsReady("rewardedVideoZone") && !Chartboost.hasRewardedVideo(CBLocation.Default)) {
            yield return new WaitForSeconds(0.5f);
        }

        m_Button.SetActive(true);

        m_Reward = Mathf.Max(20, Mathf.CeilToInt(Mathf.Sqrt(GameData.instance.coins * 10)));
        m_DataBindContext["reward"] = m_Reward;
    }
    */
}