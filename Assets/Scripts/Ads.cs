using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;

public class Ads : Singleton<Ads>
{
  [SerializeField]
  private AndroidAdIds m_AndroidAdIds;
  [SerializeField]
  private AdPosition m_BannerPosition;

  private BannerView m_BannerView;
  private InterstitialAd m_InterstitialAd;
  private RewardBasedVideoAd m_RewardedVideoAd;

  public bool isRewardedVideoReady
  {
    get {
#if UNITY_EDITOR
      return true;
#else
      return m_RewardedVideoAd.IsLoaded();
#endif
    }
  }

  private void Start()
  {
#if UNITY_ANDROID
    m_BannerView = new BannerView(m_AndroidAdIds.bannerUnitId, AdSize.Banner, m_BannerPosition);
    m_InterstitialAd = new InterstitialAd(m_AndroidAdIds.interstitialUnitId);
    m_RewardedVideoAd = RewardBasedVideoAd.Instance;

    m_RewardedVideoAd.OnAdFailedToLoad = (sender, args) =>
    {
      LoadRewardedVideo();
    };
#endif

    LoadInterstitial();
    LoadRewardedVideo();
  }

  private void OnDisable()
  {
    StopAllCoroutines();
  }

  public void ShowBanner()
  {
    var request = new AdRequest.Builder().Build();
    m_BannerView.LoadAd(request);
  }

  public void HideBanner()
  {
    m_BannerView.Hide();
  }

  private void LoadInterstitial()
  {
    var request = new AdRequest.Builder().Build();
    m_InterstitialAd.LoadAd(request);
  }

  public bool ShowInterstitial(float delay = -1.0f)
  {
    if (m_InterstitialAd.IsLoaded()) {
      if (delay <= 0.0f) {
        m_InterstitialAd.Show();
      } else {
        Invoke("ShowInterstitialInvokable", delay);
      }

      LoadInterstitial();
      return true;
    }

    LoadInterstitial();
    return false;
  }

  private void ShowInterstitialInvokable()
  {
    m_InterstitialAd.Show();
    LoadInterstitial();
  }

  public void ShowRewardedVideo(UnityAction<bool> onComplete)
  {
#if UNITY_EDITOR
    onComplete.Invoke(true);
#else
    if (m_RewardedVideoAd.IsLoaded()) {
      m_RewardedVideoAd.OnAdClosed = (sender, args) =>
      {
        onComplete.Invoke(false);
      };
      m_RewardedVideoAd.OnAdRewarded = (sender, reward) =>
      {
        onComplete.Invoke(true);
      };

      m_RewardedVideoAd.Show();
    } else {
      onComplete.Invoke(false);
    }
#endif
  }

  private void LoadRewardedVideo()
  {
#if UNITY_ANDROID
    var request = new AdRequest.Builder().Build();
    m_RewardedVideoAd.LoadAd(request, m_AndroidAdIds.rewardedVideoUnitId);
#endif
  }

  protected new void OnDestroy()
  {
    base.OnDestroy();
    m_BannerView.Destroy();
    m_InterstitialAd.Destroy();
  }

  [Serializable]
  private struct AndroidAdIds
  {
    public string bannerUnitId;
    public string interstitialUnitId;
    public string rewardedVideoUnitId;
  }
}