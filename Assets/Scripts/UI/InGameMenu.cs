using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InGameMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject inGameMenu;
    public GameObject optionsMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);                
            }
            else
            {
                inGameMenu.SetActive(!inGameMenu.activeSelf);
                if (inGameMenu.activeSelf)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1.0f;
                }
            }

        }
    }
    
    public void RestartGame()
    {
        //재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OpenOptions()
    {
        // 인게임 메뉴를 닫고 옵션 메뉴를 엽니다.
        inGameMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void CloseOptions()
    {
        // 옵션 메뉴를 닫고 인게임 메뉴를 엽니다.
        optionsMenu.SetActive(false);
        inGameMenu.SetActive(true);
    }
    public void GoToMainMenu()
    {
        //메인메뉴
        SceneManager.LoadScene("BeginingScene");
        Time.timeScale = 1.0f;
    }
    public void QuitGame()
    {
        //프로그램종료
        Application.Quit();
    }



}



