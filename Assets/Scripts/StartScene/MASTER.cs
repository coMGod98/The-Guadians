using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MASTER : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float sliderval)
    {
        mixer.SetFloat("MASTER", Mathf.Log10(sliderval) * 20);
    }
}
