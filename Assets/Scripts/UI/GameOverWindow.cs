using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour, IEventHook
{
	[SerializeField]
	private DataBindContext m_DataBindContext;
	[SerializeField]
	private float m_AdDelay;
	private float m_LastAdTime;

	public void OnInvoke(EventId eventId)
	{
		if (eventId == EventId.EndGame) {
			gameObject.SetActive(true);

			var gc = GameController.instance;
			var gd = GameData.instance;

			m_DataBindContext["lastScore"] = gc.score;
			m_DataBindContext["lastCoins"] = gc.coins;
			m_DataBindContext["bestScore"] = gd.bestScore;
			m_DataBindContext["coins"] = gd.coins;

			if (Time.time - m_LastAdTime >= m_AdDelay) {
				m_LastAdTime = Time.time;
				Ads.instance.ShowInterstitial();
			}
		}
	}

	private void OnApplicationPause(bool paused)
	{
		m_LastAdTime = Time.time;
	}

	public void OnLeaderboardsClick()
	{
		Social.ShowLeaderboardUI();
	}

	public void OnHomeClick()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene()
			.buildIndex);
	}

	public void OnRetryClick()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene()
			.buildIndex);
		PlayerPrefs.SetInt("IsRetry", 1);
		PlayerPrefs.Save();
	}

	public void OnRateClick()
	{
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.bundleIdentifier);
#endif
	}

	public void OnShareClick()
	{
		StartCoroutine(ShareCoroutine());
	}

	private IEnumerator ShareCoroutine()
	{
		m_DataBindContext["lastScore"] = GameController.instance.score;
		m_DataBindContext["lastCoins"] = GameController.instance.coins;
		m_DataBindContext["bestScore"] = GameData.instance.bestScore;

		yield return new WaitForEndOfFrame();

#if UNITY_ANDROID
        var message = string.Format(Localization.GetText("Share Message"),
            GameController.instance.score.ToString("N0"),
           "https://play.google.com/store/apps/details?id=" + Application.bundleIdentifier);
#endif
		NativeShare.ShareScreenshotWithText(message);
	}
}