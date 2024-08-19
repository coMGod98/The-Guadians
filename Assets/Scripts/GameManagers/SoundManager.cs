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


    public void MASTERSetLevel(float sliderval)
    {
        audioMixer.SetFloat("MASTER", Mathf.Log10(sliderval) * 20);
    }

    public void BGMSetLevel(float sliderval)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(sliderval) * 20);
    }

    public void SYSTEMSetLevel(float sliderval)
    {
        audioMixer.SetFloat("SYSTEM", Mathf.Log10(sliderval) * 20);
    }

    public void SFXSetLevel(float sliderval)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(sliderval) * 20);
    }
}
