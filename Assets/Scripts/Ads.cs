using ChartboostSDK;
using UnityEngine;

public class Ads : Singleton<Ads>
{
    private void Awake()
    {
        Chartboost.cacheInterstitial(CBLocation.Default);
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