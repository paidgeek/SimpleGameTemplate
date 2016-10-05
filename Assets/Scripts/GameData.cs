using UnityEngine;

public class GameData : Singleton<GameData>
{
  public int bestScore
  {
    get {
      return PlayerPrefs.GetInt("bestScore", 0);
    }
    set {
      PlayerPrefs.SetInt("bestScore", value);
      PlayerPrefs.Save();
    }
  }

  public int coins
  {
    get {
      return PlayerPrefs.GetInt("coins", 0);
    }
    set {
      PlayerPrefs.SetInt("coins", value);
      PlayerPrefs.Save();
    }
  }

  public int gamesPlayed
  {
    get {
      return PlayerPrefs.GetInt("gamesPlayed", 0);
    }
    set {
      PlayerPrefs.SetInt("gamesPlayed", value);
      PlayerPrefs.Save();
    }
  }
}