using UnityEngine;

public class InGameMenuController : MonoBehaviour, IEventHook
{
	[SerializeField]
	private DataBindContext m_DataBindContext;

	public void OnInvoke(EventId eventId)
	{
		if (eventId == EventId.StartGame) {
			gameObject.SetActive(true);

			m_DataBindContext["score"] = 0;
		} else if (eventId == EventId.EndGame) {
			gameObject.SetActive(false);
		} else if (eventId == EventId.PauseGame) {
			gameObject.SetActive(false);
		} else if (eventId == EventId.ContinueGame) {
			gameObject.SetActive(true);
		}
	}

	private void Start()
	{
		var gc = GameController.instance;

		gc.onScoreChanged.AddListener(OnScoreChanged);
	}

	private void OnScoreChanged(int score)
	{
		m_DataBindContext["score"] = score;
	}

	public void OnPauseClick()
	{
		GameController.instance.Pause();
	}
}