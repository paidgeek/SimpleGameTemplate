using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject m_HighQualityIcon;
    [SerializeField]
    private Image m_SoundIcon;
    [SerializeField]
    private Image m_ConnectedButton;
    private Color m_ConnectedColor;

    [Header("Icons")]
    [SerializeField]
    private Sprite m_SoundOnIcon;
    [SerializeField]
    private Sprite m_SoundOffIcon;

    public bool isSoundOn
    {
        get { return PlayerPrefs.GetInt("SoundToggle", 1) == 1; }
    }
    public bool isHighQuality
    {
        get { return PlayerPrefs.GetInt("HighQuality", 1) == 1; }
    }

    private void Start()
    {
        m_ConnectedColor = m_ConnectedButton.color;
    }

    private void OnEnable()
    {
        OnSoundChanged();
        OnLogInChanged();
        OnQualityChanged();
    }

    public void OnSoundClick()
    {
        PlayerPrefs.SetInt("SoundToggle", isSoundOn ? 0 : 1);
        PlayerPrefs.Save();
        OnSoundChanged();
    }

    public void OnHighQualityClick()
    {
        if (isHighQuality) {
            PlayerPrefs.SetInt("HighQuality", 0);
        } else {
            PlayerPrefs.SetInt("HighQuality", 1);
        }

        QualitySettings.SetQualityLevel(isHighQuality ? 1 : 0);

        PlayerPrefs.Save();
        OnQualityChanged();
    }

    public void OnConnectClick()
    {
#if UNITY_ANDROID
        if (GooglePlayConnection.Instance.IsConnected) {
            GooglePlayConnection.Instance.Disconnect();
            GooglePlusAPI.Instance.ClearDefaultAccount();

            OnLogInChanged();
        } else {
            GooglePlayConnection.ActionConnectionResultReceived = result =>
            {
                if (result.IsSuccess) {
                    OnLogInChanged();
                }
            };
            GooglePlayConnection.Instance.Connect();
        }
#endif
    }

    private void OnQualityChanged()
    {
        if (isHighQuality) {
            m_HighQualityIcon.gameObject.SetActive(true);
        } else {
            m_HighQualityIcon.gameObject.SetActive(false);
        }
    }

    private void OnSoundChanged()
    {
        if (isSoundOn) {
            m_SoundIcon.sprite = m_SoundOnIcon;
        } else {
            m_SoundIcon.sprite = m_SoundOffIcon;
        }
    }

    private void OnLogInChanged()
    {
#if UNITY_ANDROID
        if (GooglePlayConnection.Instance.IsConnected) {
            m_ConnectedButton.color = m_ConnectedColor;
        } else {
            m_ConnectedButton.color = Color.white;
        }
#endif
    }
}