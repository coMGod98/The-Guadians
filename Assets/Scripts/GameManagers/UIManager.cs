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
    private int[] LbuttonClicks = new int[3] { 1, 1, 1 };
    private int[] RButtonClicks = new int[4] { 1, 1, 1, 1 };
    private bool isButtonLocked = false;

    [Header("PortraitList")]
    public Sprite[] unitPortraitList;
    public Sprite[] monsterPortraitList;

    [Header("UnitDetail")]
    public GameObject showSelectedUnits;
    public GameObject showUnitDetails;
    public Image unitPortrait;
    public TextMeshProUGUI jobText;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI attackRangeText;
    public TextMeshProUGUI attackDamageText;

    public Transform portraitParent;
    public GameObject portraitSlot;

    [Header("MonsterDetail")]
    public GameObject showMonsterDetails;
    public Image monsterPortrait;
    public Slider monsterHPSlider;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;

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

    public void ShowUnitsDetails()
    {
        List<Unit> selectedUnits = GameWorld.Instance.UnitManager.selectedUnitList;

        if (selectedUnits.Count < 1)
        {
            showSelectedUnits.SetActive(false);
            showUnitDetails.SetActive(false);
            DestroyChild();
        }
        else if (selectedUnits.Count == 1)
        {
            showSelectedUnits.SetActive(false);
            showUnitDetails.SetActive(true);
            DestroyChild();

            Unit unit = selectedUnits[0];
            jobText.text = unit.unitData.job.ToString();
            rankText.text = unit.unitData.rank.ToString();
            switch (unit.unitData.job)
            {
                case UnitJob.Warrior:
                    attackSpeedText.text = "";
                    unitPortrait.sprite = unitPortraitList[0];
                    attackDamageText.text = $"{unit.unitDamage}(+{GameWorld.Instance.UnitManager.warriorUpgrade}강)";
                    break;
                case UnitJob.Archer:
                    attackSpeedText.text = "";
                    unitPortrait.sprite = unitPortraitList[1];
                    attackDamageText.text = $"{unit.unitDamage}(+{GameWorld.Instance.UnitManager.archerUpgrade}강)";
                    break;
                case UnitJob.Wizard:
                    attackSpeedText.text = "";
                    unitPortrait.sprite = unitPortraitList[2];
                    attackDamageText.text = $"{unit.unitDamage}(+{GameWorld.Instance.UnitManager.wizardUpgrade}강)";
                    break;
            }
            switch (unit.unitData.rank)
            {
                case UnitRank.Common:
                case UnitRank.Uncommon:
                case UnitRank.Rare:
                    attackRangeText.text = "";
                    break;
                case UnitRank.Epic:
                    attackRangeText.text = "";
                    break;
                case UnitRank.Legendary:
                    attackRangeText.text = "";
                    break;
            }
        }
        else
        {
            showSelectedUnits.SetActive(true);
            showUnitDetails.SetActive(false);
            DestroyChild();

            foreach (Unit unit in selectedUnits)
            {
                GameObject obj = Instantiate(portraitSlot, portraitParent);
                Image[] portrait = obj.GetComponentsInChildren<Image>();
                switch (unit.unitData.job)
                {
                    case UnitJob.Warrior:
                        portrait[1].sprite = unitPortraitList[0];
                        break;
                    case UnitJob.Archer:
                        portrait[1].sprite = unitPortraitList[1];
                        break;
                    case UnitJob.Wizard:
                        portrait[1].sprite = unitPortraitList[2];
                        break;
                }
            }
        }
    }

    public void DestroyChild()
    {
        Transform[] portraitChild = portraitParent.GetComponentsInChildren<Transform>();
        if (portraitChild != null)
        {
            for (int i = 1; i < portraitChild.Length; i++)
            {
                Destroy(portraitChild[i].gameObject);
            }
        }
    }

    public void ShowMonsterDetails()
    {
        Monster selectedMonster = GameWorld.Instance.MonsterManager.selectedMonster;
        if (selectedMonster != null)
        {
            showMonsterDetails.SetActive(true);
            monsterHPSlider.value = selectedMonster.curHP / selectedMonster.monsterData.HP;
            nameText.text = selectedMonster.monsterKey;
            hpText.text = $"{selectedMonster.curHP} / {selectedMonster.monsterData.HP}";
        }
        else
        {
            showMonsterDetails.SetActive(false);
        }
    }

    private void Buttons(int buttonIndex, int gold, int aoe, bool isLeftButton, Func<bool> canUpgrade, Action upgradeAction)
    {
        if (isButtonLocked)
        {
            Toast.Show("Skill Not Used Yet", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
            return;
        }

        if (isLeftButton && !canUpgrade())
        {
            Toast.Show("No Units Available to Upgrade", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
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
            Toast.Show($"Not Enough Gold. <size=25> \n{neededGold} Needed gold : </size>", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
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
