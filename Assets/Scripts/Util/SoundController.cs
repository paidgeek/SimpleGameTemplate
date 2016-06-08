using System.Collections.Generic;
using UnityEngine;

public class SoundController : Singleton<SoundController>
{
    private IDictionary<string, AudioClip> m_AudioClips;

    private void Start()
    {
        var clips = Resources.LoadAll<AudioClip>("Audio");
        m_AudioClips = new Dictionary<string, AudioClip>();

        for (var i = 0; i < clips.Length; i++) {
            var clip = clips[i];
            m_AudioClips[clip.name] = clip;
        }
    }

    public void Play(string clipName)
    {
        AudioSource.PlayClipAtPoint(m_AudioClips[clipName], transform.position);
    }
}
