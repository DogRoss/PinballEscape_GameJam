using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipManager : MonoBehaviour
{
    public AudioObj[] audioClips;

    List<AudioObj> backgroundMusic;

    private void Awake()
    {
        backgroundMusic = new List<AudioObj>();
        foreach (AudioObj a in audioClips)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.clip;
            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
            a.source.outputAudioMixerGroup = a.group;
            if (a.isBackgroundMusic) backgroundMusic.Add(a);
        }
        StartCoroutine(LoopBackgroundMusic());
    }

    public void PlayClip(string name) => Array.Find(audioClips, sound => sound.name == name).source.Play();

    public void StopClip(string name) => Array.Find(audioClips, sound => sound.name == name).source.Stop();

    IEnumerator LoopBackgroundMusic()
    {
        int index = 0;
        while (true)
        {
            PlayClip(backgroundMusic[index].name);
            yield return new WaitForSeconds(backgroundMusic[index].clip.length);
            index++;
            if (index >= backgroundMusic.Count) index = 0;
        }
    }
}
