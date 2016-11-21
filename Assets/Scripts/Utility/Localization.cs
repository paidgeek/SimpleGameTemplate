using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Localization
{
  private static IDictionary<string, string> s_Content;
  private static string s_Language = "EN";

  private static string Language
  {
    get {
      return s_Language;
    }
    set {
      if (s_Language != value) {
        s_Language = value;
        CreateContent();
      }
    }
  }

  private static IDictionary<string, string> Content
  {
    get {
      if (s_Content == null) {
        CreateContent();
      }

      return s_Content;
    }
  }

  public static string GetText(string key)
  {
    var result = "";
    Content.TryGetValue(key, out result);

    if (string.IsNullOrEmpty(result)) {
      return key + "[" + Language + "]" + " No Text defined";
    }

    return result;
  }

  public static string GetLanguage()
  {
    return Language;
  }

  public static void SetLanguage(string language)
  {
    Language = language;
  }

  private static IDictionary<string, string> GetContent()
  {
    if (s_Content == null || s_Content.Count == 0) {
      CreateContent();
    }

    return s_Content;
  }

  private static void CreateContent()
  {
    s_Content = new Dictionary<string, string>();

    var assets = Resources.LoadAll("Localization/" + s_Language);
    foreach (var asset in assets) {
      var source = (TextAsset) asset;
      ParseLocalization(Encoding.UTF8.GetString(source.bytes));
    }
  }

  private static void ParseLocalization(string source)
  {
    var lines = source.Split(new[] {
      "\n",
      "\r"
    }, StringSplitOptions.RemoveEmptyEntries);

    for (var i = 0; i < lines.Length; i++) {
      var line = lines[i].Trim();

      if (!line.StartsWith("#") && !string.IsNullOrEmpty(line)) {
        var keyEnd = line.IndexOf(':');
        var key = line.Substring(0, keyEnd).Trim();
        var value = line.Substring(keyEnd + 1).Trim();

        s_Content[key] = value;
      }
    }
  }
}