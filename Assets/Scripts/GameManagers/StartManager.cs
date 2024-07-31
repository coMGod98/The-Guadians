using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
