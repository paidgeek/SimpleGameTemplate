using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : Singleton<DialogController>
{
    [SerializeField]
    private Text m_Text;
    [SerializeField]
    private Text m_LeftText;
    [SerializeField]
    private Text m_RightText;

    private System.Action m_OnLeftClick;
    private System.Action m_OnRightClick;

    public void Show(string text, string leftText, string rightText, System.Action onLeftClick, System.Action onRightClick)
    {
        gameObject.SetActive(true);

        m_Text.text = text;
        m_LeftText.text = leftText;
        m_RightText.text = rightText;
        m_OnLeftClick = onLeftClick;
        m_OnRightClick = onRightClick;
    }

    public void OnLeftClick()
    {
        gameObject.SetActive(false);

        if (m_OnLeftClick != null) {
            m_OnLeftClick.Invoke();
        }
    }

    public void OnRightClick()
    {
        gameObject.SetActive(false);

        if (m_OnRightClick != null) {
            m_OnRightClick.Invoke();
        }
    }
}