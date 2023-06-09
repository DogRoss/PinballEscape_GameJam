using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Audio", menuName = "Sounds/New Audio")]
public class AudioObj : ScriptableObject
{
    public AudioClip clip;
    public string clipName;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;
    public bool isBackgroundMusic;

    public AudioMixerGroup group;

    [HideInInspector]
    public AudioSource source;
}
