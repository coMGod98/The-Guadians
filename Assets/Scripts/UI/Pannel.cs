using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pannel : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneLoader.LoadScene(1);
            Time.timeScale = 1.0f;
        }
    }
}
