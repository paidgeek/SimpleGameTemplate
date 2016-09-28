using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
	public bool isSoundOn
	{
		set {
			PlayerPrefs.SetInt("SoundToggle", value ? 1 : 0);
			PlayerPrefs.Save();
		}
		get { return PlayerPrefs.GetInt("SoundToggle", 1) == 1; }
	}

	public bool isHighQuality
	{
		set {
			PlayerPrefs.SetInt("HighQuality", value ? 1 : 0);
			PlayerPrefs.Save();
		}
		get { return PlayerPrefs.GetInt("HighQuality", 1) == 1; }
	}
}