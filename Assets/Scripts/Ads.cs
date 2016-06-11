using ChartboostSDK;
using GoogleMobileAds.Api;
using UnityEngine;

public class Ads : Singleton<Ads>
{
    [SerializeField]
    private string m_AndroidBannerUnitId;
    private BannerView m_BannerView;

    private void Awake()
    {
        Chartboost.cacheInterstitial(CBLocation.Default);
    }

    public void ShowBanner()
    {
#if UNITY_ANDROID
        string adUnitId = m_AndroidBannerUnitId;
#elif UNITY_IPHONE
        string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
#else
        string adUnitId = "unexpected_platform";
#endif

        m_BannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        m_BannerView.LoadAd(request);
    }

    public void HideBanner()
    {
        m_BannerView.Destroy();
    }

    public void ShowInterstitial()
    {
        Debug.Log("Showing Chartboost interstitial");

        if (Chartboost.hasInterstitial(CBLocation.Default)) {
            Chartboost.showInterstitial(CBLocation.Default);
        }

        Chartboost.cacheInterstitial(CBLocation.Default);
    }
}