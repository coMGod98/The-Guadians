using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SYSTEM : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float sliderval)
    {
        mixer.SetFloat("SYSTEM", Mathf.Log10(sliderval) * 20);
    }
}
