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
        StartCoroutine(LoadAdCoroutine());
    }

    public void OnClick()
    {
        if (!Advertisement.IsReady()) {
            return;
        }

        m_Button.SetActive(false);

        var opts = new ShowOptions();
        opts.resultCallback = result =>
        {
            if (result == ShowResult.Finished) {
                GameData.instance.coins += m_Reward;
                m_DataBindContext["coins"] = GameData.instance.coins;
            }

            StartCoroutine(LoadAdCoroutine());
        };
        Advertisement.Show("rewardedVideoZone", opts);
    }

    private IEnumerator LoadAdCoroutine()
    {
        while (!Advertisement.IsReady("rewardedVideoZone")) {
            yield return new WaitForSeconds(1.0f);
        }

        m_Button.SetActive(true);

        m_Reward = Mathf.Max(20, Mathf.CeilToInt(Mathf.Sqrt(GameData.instance.coins * 10)));
        m_DataBindContext["reward"] = m_Reward;
    }
}