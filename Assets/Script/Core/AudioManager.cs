using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public struct AudioCue
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoSingleton<AudioManager>
{
    public List<AudioCue> audioCues;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioCue(string cueName, bool isLoop)
    {
        AudioCue audioCue = audioCues.Find(cue => cue.name == cueName);

        if (audioCue.clip != null)
        {
            audioSource.clip = audioCue.clip;
            audioSource.loop = isLoop;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio cue not found: " + cueName);
        }
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
