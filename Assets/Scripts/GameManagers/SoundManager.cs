using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioClip warrior;
    public AudioClip archer;
    public AudioClip wizard;
    public AudioClip gold;
    public AudioMixer audioMixer;
    public AudioSource goldAudio;


    void Start()
    {
        AudioSource[] temp = GetComponentsInChildren<AudioSource>();
        goldAudio = temp[1];
    }
}
