using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject gameMenu;
    public GameObject optionsMenu;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void GoToMain()
    {
        SceneLoader.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void Option()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
