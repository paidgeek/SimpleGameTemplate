using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dialog : Singleton<Dialog>
{
  [SerializeField]
  private Text m_Text;
  [SerializeField]
  private Text[] m_ButtonTexts;
  private UnityAction<int> m_Action;

  public void Show(string text, string[] buttons, UnityAction<int> action)
  {
    if (buttons.Length == 0) {
      Debug.LogError("Invalid button count");
      return;
    }

    for (var i = 0; i < m_ButtonTexts.Length; i++) {
      m_ButtonTexts[i].transform.parent.gameObject.SetActive(i < buttons.Length);
    }
    gameObject.SetActive(true);

    m_Text.text = text;
    m_Action = action;

    for (var i = 0; i < buttons.Length; i++) {
      m_ButtonTexts[i].text = buttons[i];
    }
  }

  public void OnClick(int button)
  {
    gameObject.SetActive(false);

    if (m_Action != null) {
      m_Action.Invoke(button);
    }
  }
}