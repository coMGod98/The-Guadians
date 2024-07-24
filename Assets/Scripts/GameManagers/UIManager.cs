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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        GoldUI();
    }

    private void UpdateUI()
    {
        timerText.text = (GameWorld.Instance.remainTime < 10 ? GameWorld.Instance.remainTime.ToString("F1") : GameWorld.Instance.remainTime.ToString("F0"));
        roundText.text = $"{GameWorld.Instance.curRound.ToString()}/{GameWorld.Instance.totalRounds.ToString()}";
        monsterCountText.text = "" + GameWorld.Instance.MonsterManager.allMonsterList.Count;
    }

    private void GoldUI()
    {
        curGold.text = "" + GameWorld.Instance.playerGolds.ToString();
    }

    public void LButtons(int LbuttonIndex, int gold)
    {
        if (GameWorld.Instance.playerGolds >= LbuttonClicks[LbuttonIndex] * gold)
        {
            LbuttonClicks[LbuttonIndex]++;
            GameWorld.Instance.TakeGold(LbuttonClicks[LbuttonIndex] * gold);
        }
        else
        {
            int neededGold = (LbuttonClicks[LbuttonIndex] * gold) - GameWorld.Instance.playerGolds;
            Toast.Show("골드가 부족합니다. <size=25> \n" + neededGold.ToString() + " 골드가 더 필요합니다 </size> ", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
        }
    }

    public void RButtons(int RbuttonIndex, int gold, int aoe)
    {
        if (GameWorld.Instance.playerGolds >= RButtonClicks[RbuttonIndex] * gold)
        {
            RButtonClicks[RbuttonIndex]++;
            GameWorld.Instance.TakeGold(RButtonClicks[RbuttonIndex] * gold);
            GameWorld.Instance.AoeManager.OnAoeButtonClicked(aoe);
        }
        else
        {
            int neededGold = (RButtonClicks[RbuttonIndex] * gold) - GameWorld.Instance.playerGolds;
            Toast.Show("골드가 부족합니다. <size=25> \n" + neededGold.ToString() + " 골드가 더 필요합니다 </size> ", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
        }
    }

    public void LButton1()
    {
        LButtons(0, 5);
    }

    public void LButton2()
    {
        LButtons(1, 10);
    }

    public void LButton3()
    {
        LButtons(2, 15);
    }

    public void RButton1()
    {
        RButtons(0, 5, 1);
    }

    public void RButton2()
    {
        RButtons(1, 10, 2);
    }

    public void RButton3()
    {
        RButtons(2, 15, 3);
    }

    public void addButton1()
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
