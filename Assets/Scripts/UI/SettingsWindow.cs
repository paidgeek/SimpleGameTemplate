using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject m_HighQualityIcon;
    [SerializeField]
    private Image m_SoundIcon;
    [SerializeField]
    private Image m_ConnectButton;
    [SerializeField]
    private Color m_ConnectedButtonTint;
    private GameSettings m_GameSettings;

    [Header("Icons")]
    [SerializeField]
    private Sprite m_SoundOnIcon;
    [SerializeField]
    private Sprite m_SoundOffIcon;

    private void OnEnable()
    {
        m_GameSettings = GameSettings.instance;

        OnSoundChanged();
        OnLogInChanged();
        OnQualityChanged();
    }

    public void OnSoundClick()
    {
        m_GameSettings.isSoundOn = !m_GameSettings.isSoundOn;
        OnSoundChanged();
    }

    public void OnHighQualityClick()
    {
        m_GameSettings.isHighQuality = !m_GameSettings.isHighQuality;
        QualitySettings.SetQualityLevel(m_GameSettings.isHighQuality ? 1 : 0);

        OnQualityChanged();
    }

    public void OnConnectClick()
    {
#if UNITY_ANDROID
        if (GooglePlayConnection.Instance.IsConnected) {
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
        if (m_GameSettings.isHighQuality) {
            m_HighQualityIcon.gameObject.SetActive(true);
        } else {
            m_HighQualityIcon.gameObject.SetActive(false);
        }
    }

    private void OnSoundChanged()
    {
        if (m_GameSettings.isSoundOn) {
            m_SoundIcon.sprite = m_SoundOnIcon;
        } else {
            m_SoundIcon.sprite = m_SoundOffIcon;
        }
    }

    private void OnLogInChanged()
    {
#if UNITY_ANDROID
        if (GooglePlayConnection.Instance.IsConnected) {
            m_ConnectButton.color = m_ConnectedButtonTint;
        } else {
            m_ConnectButton.color = Color.white;
        }
#endif
    }
}