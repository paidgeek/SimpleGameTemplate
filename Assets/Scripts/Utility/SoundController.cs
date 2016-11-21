using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : Singleton<SoundController>
{
  private AudioSource m_AudioSource;
  private IDictionary<string, AudioClip> m_AudioClips;

  private void Start()
  {
    m_AudioSource = GetComponent<AudioSource>();
    var clips = Resources.LoadAll<AudioClip>("Audio");
    m_AudioClips = new Dictionary<string, AudioClip>();

    for (var i = 0; i < clips.Length; i++) {
      var clip = clips[i];
      m_AudioClips[clip.name] = clip;
    }
  }

  public void Play(string clipName)
  {
    if (m_AudioClips == null || !GameSettings.instance.isSoundOn) {
      return;
    }

    m_AudioSource.PlayOneShot(m_AudioClips[clipName]);
  }
}