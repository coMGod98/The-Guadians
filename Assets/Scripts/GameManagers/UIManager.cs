using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.Toast;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField] ShowDetails _showDetails;
    //[SerializeField] 
    public ShowDetails ShowDetails => _showDetails;


    private int[] LbuttonClicks = new int[3] { 1, 1, 1 };
    private int[] RButtonClicks = new int[4] { 1, 1, 1, 1 };
    private bool isButtonLocked = false;


    [Header("Info")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI monsterCountText;
    public TextMeshProUGUI curGold;

    [Header("Panel")]
    public GameObject gameWin;
    public GameObject gameLost;

    private void Start()
    {
    }

    private void Update()
    {
        UpdateUI();
        _showDetails.MonsterDetails();
        _showDetails.BossDetails();
    }

    private void UpdateUI()
    {
        timerText.text = (GameWorld.Instance.remainTime < 10 ? GameWorld.Instance.remainTime.ToString("F1") : GameWorld.Instance.remainTime.ToString("F0"));
        roundText.text = $"{GameWorld.Instance.curRound.ToString()}/{GameWorld.Instance.totalRounds.ToString()}";
        monsterCountText.text = "" + GameWorld.Instance.MonsterManager.allMonsterList.Count;
        curGold.text = "" + GameWorld.Instance.playerGolds.ToString();
    }



    private void Buttons(int buttonIndex, int gold, int aoe, bool isLeftButton, Func<bool> canUpgrade, Action upgradeAction)
    {
        if (isButtonLocked)
        {
            Toast.Show("Skill Not Used Yet", 2f, ToastColor.Black, ToastPosition.MiddleCenter); // 버튼 누르고 스킬 사용 안했을떄 나오는 토스트 메시지
            return;
        }

        if (isLeftButton && !canUpgrade())
        {
            Toast.Show("No Units Available to Upgrade", 2f, ToastColor.Black, ToastPosition.MiddleCenter); // 업그레이드 할 유닛이 없을때 나오는 토스트 메시지
            return;
        }

        int[] buttonClicks = isLeftButton ? LbuttonClicks : RButtonClicks;
        Action<int> onButtonClick = isLeftButton ? (Action<int>)null : GameWorld.Instance.AoeManager.ButtonClick;
        int requiredGold = buttonClicks[buttonIndex] * gold;
        if (GameWorld.Instance.playerGolds >= requiredGold)
        {
            GameWorld.Instance.TakeGold(requiredGold);
            buttonClicks[buttonIndex]++;
            LockButtons();
            onButtonClick?.Invoke(aoe);

            if (isLeftButton)
            {
                upgradeAction?.Invoke();
            }

            if (isLeftButton)
            {
                isButtonLocked = false;
            }
            else
            {
                GameWorld.Instance.AoeManager.AoePlaced += UnlockButtons;
            }
        }
        else
        {
            int neededGold = requiredGold - GameWorld.Instance.playerGolds;
            Toast.Show($"Not Enough Gold. <size=25> \n{neededGold} Needed gold : </size>", 2f, ToastColor.Black, ToastPosition.MiddleCenter); // 골드가 부족할때 나오는 토스트 메시지
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

    public void LButtons(int index, int gold, Func<bool> canUpgrade, Action upgradeAction)
    {
        Buttons(index, gold, 0, true, canUpgrade, upgradeAction);
    }

    public void RButtons(int index, int gold, int aoe)
    {
        Buttons(index, gold, aoe, false, null, null);
    }

    public void LButton1() => LButtons(0, 5, () => GameWorld.Instance.UnitManager.allUnitList.Any(unit => unit.unitData.job == UnitJob.Warrior), () => GameWorld.Instance.UnitManager.warriorUpgrade++);
    public void LButton2() => LButtons(1, 10, () => GameWorld.Instance.UnitManager.allUnitList.Any(unit => unit.unitData.job == UnitJob.Archer), () => GameWorld.Instance.UnitManager.archerUpgrade++);
    public void LButton3() => LButtons(2, 15, () => GameWorld.Instance.UnitManager.allUnitList.Any(unit => unit.unitData.job == UnitJob.Wizard), () => GameWorld.Instance.UnitManager.wizardUpgrade++);

    public void RButton1() => RButtons(0, 5, 0);
    public void RButton2() => RButtons(1, 10, 1);
    public void RButton3() => RButtons(2, 15, 2);
    public void RButton4() => RButtons(3, 20, 3);

    public void GameState(bool isState)
    {
        gameWin.SetActive(isState);
        gameLost.SetActive(!isState);
    }

    public void addGold()
    {
        GameWorld.Instance.AddGold(100);
    }
}
