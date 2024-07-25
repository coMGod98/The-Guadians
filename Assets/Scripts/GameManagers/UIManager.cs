using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyUI.Toast;
using System;

public class UIManager : MonoBehaviour
{
    private int[] LbuttonClicks = new int[3] { 1, 2, 3 };
    private int[] RButtonClicks = new int[3] { 1, 2, 3 };
    private bool isButtonLocked = false;

    [Header("Info")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI monsterCountText;

    [Header("Panel")]
    public GameObject gameWin;
    public GameObject gameLost;

    [Header("Gold")]
    public TextMeshProUGUI curGold;


    private void Start()
    {

    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        timerText.text = (GameWorld.Instance.remainTime < 10 ? GameWorld.Instance.remainTime.ToString("F1") : GameWorld.Instance.remainTime.ToString("F0"));
        roundText.text = $"{GameWorld.Instance.curRound.ToString()}/{GameWorld.Instance.totalRounds.ToString()}";
        monsterCountText.text = "" + GameWorld.Instance.MonsterManager.allMonsterList.Count;
        curGold.text = "" + GameWorld.Instance.playerGolds.ToString();
    }

    private void Buttons(int buttonIndex, int gold, int aoe, bool isLeftButton)
    {
        if (isButtonLocked)
        {
            Toast.Show("스킬이 사용되지 않았습니다", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
            return;
        }

        int[] buttonClicks = isLeftButton ? LbuttonClicks : RButtonClicks;
        Action<int> onButtonClick = isLeftButton ? (Action<int>)null : GameWorld.Instance.AoeManager.ButtonClick;

        if (GameWorld.Instance.playerGolds >= buttonClicks[buttonIndex] * gold)
        {
            buttonClicks[buttonIndex]++;
            GameWorld.Instance.TakeGold(buttonClicks[buttonIndex] * gold);
            LockButtons();
            onButtonClick?.Invoke(aoe);
            GameWorld.Instance.AoeManager.AoePlaced += UnlockButtons;
        }
        else
        {
            int neededGold = (buttonClicks[buttonIndex] * gold) - GameWorld.Instance.playerGolds;
            Toast.Show($"골드가 부족합니다. <size=25> \n{neededGold} 골드가 더 필요합니다 </size>", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
        }
    }

    private void LockButtons()
    {
        isButtonLocked = true;
    }

    private void UnlockButtons()
    {
        isButtonLocked = false;
        GameWorld.Instance.AoeManager.AoePlaced -= UnlockButtons;
    }




    public void LButtons(int index, int gold)
    {
        Buttons(index, gold, 0, true);
    }

    public void RButtons(int index, int gold, int aoe)
    {
        Buttons(index, gold, aoe, false);
    }

    // (버튼, 골드량, AOE)
    public void LButton1() => LButtons(0, 5);
    public void LButton2() => LButtons(1, 10);
    public void LButton3() => LButtons(2, 15);

    public void RButton1() => RButtons(0, 5, 0);
    public void RButton2() => RButtons(1, 10, 1);
    public void RButton3() => RButtons(2, 15, 2);
    public void RButton4() => RButtons(3, 20, 3);

    public void GameState(bool isState)
    {
        gameWin.SetActive(isState);
        gameLost.SetActive(!isState);
    }

    /*public void addGold()
    {
        GameWorld.Instance.AddGold(100);
    }*/
}
