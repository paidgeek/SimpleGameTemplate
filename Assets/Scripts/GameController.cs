using UnityEngine;

public class GameController : Singleton<GameController>
{
	private EventDispatcher m_EventDispatcher;
	private int m_Coins;
	private int m_Score;

	public int coins
	{
		get { return m_Coins; }
		set {
			m_Coins = value;
			onCoinsChanged.Invoke(m_Coins);
		}
	}

	public int score
	{
		get { return m_Score; }
		set {
			m_Score = value;
			onScoreChanged.Invoke(m_Score);
		}
	}

	public bool isNewRecord { get; private set; }
	public bool isGameOver { get; private set; }

	public BasicEvent<int> onScoreChanged { get; set; }
	public BasicEvent<int> onCoinsChanged { get; set; }

	private void Awake()
	{
		m_EventDispatcher = FindObjectOfType<EventDispatcher>();
		onScoreChanged = new BasicEvent<int>();
		onCoinsChanged = new BasicEvent<int>();
		isGameOver = true;
	}

	private void Start()
	{
		if (PlayerPrefs.GetInt("IsRetry", 0) == 1) {
			PlayerPrefs.DeleteKey("IsRetry");
			PlayerPrefs.Save();

			StartGame();
		}
	}

	public void StartGame()
	{
		score = 0;
		isGameOver = false;

		m_EventDispatcher.Invoke(EventId.StartGame);
	}

	public void EndGame()
	{
		if (isGameOver) {
			return;
		}

		StopAllCoroutines();
		isGameOver = true;

		var gd = GameData.instance;

		if (score > gd.bestScore) {
			isNewRecord = true;
			gd.bestScore = score;
			ReportScore();
		} else {
			isNewRecord = false;
		}

		gd.coins += m_Coins;
		gd.gamesPlayed++;

		m_EventDispatcher.Invoke(EventId.EndGame);
	}

	private void OnApplicationPause(bool paused)
	{
		if (isGameOver) {
			return;
		}

		if (paused) {
			Pause();
		}
	}

	public void Pause()
	{
		m_EventDispatcher.Invoke(EventId.PauseGame);
	}

	public void Continue()
	{
		m_EventDispatcher.Invoke(EventId.ContinueGame);
	}

	public void ReportScore()
	{
		Social.ReportScore(score, GooglePlayIds.leaderboard_high_scores, success =>
		{
			if (!success) {
				PlayerPrefs.SetInt("UnreportedScore", score);
				PlayerPrefs.Save();
			}
		});
	}
}