using System.Collections;
using ChartboostSDK;
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
        Chartboost.cacheRewardedVideo(CBLocation.GameOver);
        StartCoroutine(LoadAdCoroutine());
    }

    public void OnClick()
    {
        if (!Advertisement.IsReady("rewardedVideoZone") && !Chartboost.hasRewardedVideo(CBLocation.GameOver)) {
            return;
        }

        m_Button.SetActive(false);

        if (Chartboost.hasRewardedVideo(CBLocation.GameOver)) {
            Chartboost.willDisplayVideo = location =>
            {
                // TODO: mute sounds
            };
            Chartboost.didCompleteRewardedVideo = (location, reward) =>
            {
                OnCompleteVideo();
            };
            Chartboost.showRewardedVideo(CBLocation.GameOver);
            Chartboost.cacheRewardedVideo(CBLocation.GameOver);
        } else {
            var opts = new ShowOptions();
            opts.resultCallback = result =>
            {
                if (result == ShowResult.Finished) {
                    OnCompleteVideo();
                }

                StartCoroutine(LoadAdCoroutine());
            };
            Advertisement.Show("rewardedVideoZone", opts);
        }
    }

    private void OnCompleteVideo()
    {
        GameData.instance.coins += m_Reward;
        m_DataBindContext["coins"] = GameData.instance.coins;
    }

    private IEnumerator LoadAdCoroutine()
    {
        while (!Advertisement.IsReady("rewardedVideoZone") && !Chartboost.hasRewardedVideo(CBLocation.GameOver)) {
            yield return new WaitForSeconds(0.5f);
        }

        m_Button.SetActive(true);

        m_Reward = Mathf.Max(20, Mathf.CeilToInt(Mathf.Sqrt(GameData.instance.coins * 10)));
        m_DataBindContext["reward"] = m_Reward;
    }
}