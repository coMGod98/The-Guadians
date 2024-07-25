using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyUI.Toast;
using System;

public class UIManager : MonoBehaviour
{
    private int[] LbuttonClicks = new int[3];
    private int[] RButtonClicks = new int[3];

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
        int[] buttonClicks = isLeftButton ? LbuttonClicks : RButtonClicks;
        Action<int> onButtonClick = isLeftButton ? (Action<int>)null : GameWorld.Instance.AoeManager.OnAoeButtonClicked;

        if (GameWorld.Instance.playerGolds >= buttonClicks[buttonIndex] * gold)
        {
            buttonClicks[buttonIndex]++;
            GameWorld.Instance.TakeGold(buttonClicks[buttonIndex] * gold);
            onButtonClick?.Invoke(aoe);
        }
        else
        {
            int neededGold = (buttonClicks[buttonIndex] * gold) - GameWorld.Instance.playerGolds;
            Toast.Show($"골드가 부족합니다. <size=25> \n{neededGold} 골드가 더 필요합니다 </size>", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
        }
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

    public void RButton1() => RButtons(0, 5, 1);
    public void RButton2() => RButtons(1, 10, 2);
    public void RButton3() => RButtons(2, 15, 3);

    public void addGold()
    {
        GameWorld.Instance.AddGold(100);
    }

    public void GameOver()
    {
        gameLost.SetActive(true);
    }

    public void GameVictory()
    {
        gameWin.SetActive(true);
    }
}
