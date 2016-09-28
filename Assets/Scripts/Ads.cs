using System;

public class Ads : Singleton<Ads>
{
	public bool isRewardedVideoReady {
		get { return AdTapsy.IsRewardedVideoReadyToShow(); }
	}

	private void Awake()
	{
		AdTapsy.SetRewardedVideoPostPopupEnabled(false);
		AdTapsy.SetRewardedVideoPrePopupEnabled(false);
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	public bool ShowInterstitial(float delay = -1.0f)
	{
	
		if (AdTapsy.IsInterstitialReadyToShow()) {
			if (delay <= 0.0f) {
				AdTapsy.ShowInterstitial();
			} else {
				Invoke("ShowInterstitialInvokable", delay);
			}

			return true;
		}
		
		return false;
	}

	private void ShowInterstitialInvokable()
	{
		AdTapsy.ShowInterstitial();
	}

	public void ShowRewardedVideo(Action<bool> onReward)
	{
	
		if (AdTapsy.IsRewardedVideoReadyToShow()) {
			AdTapsy.OnRewardEarned = zoneId =>
			{
				onReward.Invoke(true);
			};
			AdTapsy.OnAdSkipped = zoneId =>
			{
				onReward.Invoke(false);
			};

			AdTapsy.ShowRewardedVideo();
		} else {
			onReward.Invoke(false);
		}
		
	}
}