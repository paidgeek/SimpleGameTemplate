using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Countdown : Singleton<Countdown>
{
  [SerializeField]
  private DataBindContext m_DataBindContext;

  private int m_Timer;
  private UnityAction m_OnDone;

  public void DoCountdown(int seconds, UnityAction onDone)
  {
    transform.GetChild(0)
      .gameObject.SetActive(true);
    m_Timer = seconds;
    m_OnDone = onDone;
    StartCoroutine(CountdownCoroutine());
  }

  public void CancelCountdown()
  {
    transform.GetChild(0)
      .gameObject.SetActive(false);
    StopAllCoroutines();
  }

  private IEnumerator CountdownCoroutine()
  {
    while (m_Timer > 0) {
      m_DataBindContext["countdown"] = m_Timer;

      yield return StartCoroutine(WaitForRealSeconds(1.0f));

      m_Timer--;
    }

    transform.GetChild(0)
      .gameObject.SetActive(false);
    m_OnDone.Invoke();
  }

  private IEnumerator WaitForRealSeconds(float time)
  {
    var start = Time.realtimeSinceStartup;

    while (Time.realtimeSinceStartup < start + time) {
      yield return null;
    }
  }
}