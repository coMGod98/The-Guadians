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

<<<<<<< Updated upstream
=======
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




>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
=======
    public void ShowDetails()
    {
        List<Unit> selectedUnits = GameWorld.Instance.UnitManager.selectedUnitList;
        Monster selectedMonster = GameWorld.Instance.MonsterManager.selectedMonster;

        if(selectedMonster != null)
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
            switch(unit.unitData.job)
            {
                case UnitJob.Warrior:
                attackSpeedText.text = "보통";
                unitPortrait.sprite = unitPortraitList[0];
                attackDamageText.text = $"{unit.unitDamage}(+{GameWorld.Instance.UnitManager.warriorUpgrade}강)";
                break;
                case UnitJob.Archer:
                attackSpeedText.text = "빠름";
                unitPortrait.sprite = unitPortraitList[1];
                attackDamageText.text = $"{unit.unitDamage}(+{GameWorld.Instance.UnitManager.archerUpgrade}강)";
                break;
                case UnitJob.Wizard:
                attackSpeedText.text = "느림";
                unitPortrait.sprite = unitPortraitList[2];
                attackDamageText.text = $"{unit.unitDamage}(+{GameWorld.Instance.UnitManager.wizardUpgrade}강)";
                break;
            }
            switch(unit.unitData.rank)
            {
                case UnitRank.Common:
                case UnitRank.Uncommon:
                case UnitRank.Rare:
                attackRangeText.text = "짧음";
                break;
                case UnitRank.Epic:
                attackRangeText.text = "보통";
                break;
                case UnitRank.Legendary:
                attackRangeText.text = "넓음";
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

    public void UpgradeWarrior()
    {
        GameWorld.Instance.playerGolds -= 5;
        GameWorld.Instance.UnitManager.warriorUpgrade++;
    }
    public void UpgradeArcher()
    {
        GameWorld.Instance.playerGolds -= 5;
        GameWorld.Instance.UnitManager.archerUpgrade++;
    }
    public void UpgradeWizard()
    {
        GameWorld.Instance.playerGolds -= 5;
        GameWorld.Instance.UnitManager.wizardUpgrade++;
    }




>>>>>>> Stashed changes
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
