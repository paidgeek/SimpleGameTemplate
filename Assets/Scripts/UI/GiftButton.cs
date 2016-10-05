using System.Collections;
using UnityEngine;

public class GiftButton : MonoBehaviour
{
  [SerializeField]
  private GameObject m_Button;
  [SerializeField]
  private DataBindContext m_DataBindContext;
  private int m_Reward;

  private void OnEnable()
  {
    m_Button.SetActive(false);
    StartCoroutine(WaitForVideoAd());
  }

  private IEnumerator WaitForVideoAd()
  {
    while (!Ads.instance.isRewardedVideoReady) {
      yield return new WaitForSeconds(0.3f);
    }

    m_Button.SetActive(true);
    m_Reward = Mathf.Max(20, Mathf.CeilToInt(Mathf.Sqrt(GameData.instance.coins * 10)));
    m_DataBindContext["reward"] = m_Reward;
  }

  public void OnClick()
  {
    if (!Ads.instance.isRewardedVideoReady) {
      return;
    }

    m_Button.SetActive(false);

#if UNITY_EDITOR
    GameData.instance.coins += m_Reward;
    m_DataBindContext["coins"] = GameData.instance.coins;
#else
        Ads.instance.ShowRewardedVideo(success =>
        {
            if (success) {
                GameData.instance.coins += m_Reward;
                m_DataBindContext["coins"] = GameData.instance.coins;
            }
        });
#endif
  }
}