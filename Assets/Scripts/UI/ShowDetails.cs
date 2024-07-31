using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowDetails : MonoBehaviour
{
    [Header("Portrait")]
    public Transform portraitParent;
    public GameObject portraitSlot;
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

    [Header("MonsterDetail")]
    public GameObject showMonsterDetails;
    public Image monsterPortrait;
    public Slider monsterHPSlider;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;

    [Header("BossDetail")]
    public GameObject showBossDetails;
    public Slider bossHPSlider;

    public void MonsterDetails()
    {
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
    }

    public void BossDetails()
    {
        List<Monster> allMonsterList = GameWorld.Instance.MonsterManager.allMonsterList;
        Monster boss = null;

        foreach(Monster monster in allMonsterList)
        {
            if (monster.monsterType == MonsterType.Boss)
            {
                boss = monster;
            }
        }
        if (boss != null)
        {
            showBossDetails.SetActive(true);
            bossHPSlider.value = boss.curHP / boss.monsterData.HP;
        }
        else
        {
            showBossDetails.SetActive(false);
        }

    }
    
    public void UnitDetails()
    {
        List<Unit> selectedUnits = GameWorld.Instance.UnitManager.selectedUnitList;

        if (selectedUnits.Count > 1)
        {
            showSelectedUnits.SetActive(true);
            showUnitDetails.SetActive(false);
            DestroyChild();
            int count = 0;

            foreach (Unit unit in selectedUnits)
            {
                if (count < 12)
                {
                    GameObject obj = Instantiate(portraitSlot, portraitParent);

                    PortraitSlot slot = obj.GetComponent<PortraitSlot>();
                    slot.likedUnit = unit;
                    slot.idx = count;
                    
                    Button button = obj.GetComponent<Button>();
                    button.onClick.AddListener(() => ShowClickedUnitsDetails(slot.idx));

                    Image portrait = obj.GetComponent<Image>();
                    switch (unit.unitData.job)
                    {
                        case UnitJob.Warrior:
                            portrait.sprite = unitPortraitList[0];
                            break;
                        case UnitJob.Archer:
                            portrait.sprite = unitPortraitList[1];
                            break;
                        case UnitJob.Wizard:
                            portrait.sprite = unitPortraitList[2];
                            break;
                    }
                }
                count++;
            }
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
                    attackSpeedText.text = "Normal";
                    unitPortrait.sprite = unitPortraitList[0];
                    attackDamageText.text = $"{unit.unitDamage}(+{GameWorld.Instance.UnitManager.warriorUpgrade}강)";
                    break;
                case UnitJob.Archer:
                    attackSpeedText.text = "Fast";
                    unitPortrait.sprite = unitPortraitList[1];
                    attackDamageText.text = $"{unit.unitDamage}(+{GameWorld.Instance.UnitManager.archerUpgrade}강)";
                    break;
                case UnitJob.Wizard:
                    attackSpeedText.text = "Slow";
                    unitPortrait.sprite = unitPortraitList[2];
                    attackDamageText.text = $"{unit.unitDamage}(+{GameWorld.Instance.UnitManager.wizardUpgrade}강)";
                    break;
            }
            switch (unit.unitData.rank)
            {
                case UnitRank.Common:
                case UnitRank.Uncommon:
                case UnitRank.Rare:
                    attackRangeText.text = "Short";
                    break;
                case UnitRank.Epic:
                    attackRangeText.text = "Long";
                    break;
                case UnitRank.Legendary:
                    attackRangeText.text = "High";
                    break;
            }
        }
        else
        {
            showSelectedUnits.SetActive(false);
            showUnitDetails.SetActive(false);
            DestroyChild();
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

    public void ShowClickedUnitsDetails(int index)
    {
        GameWorld.Instance.InputManager.DeselectAll();

        PortraitSlot[] portraitSlots = portraitParent.GetComponentsInChildren<PortraitSlot>();
        foreach(PortraitSlot portraitSlot in portraitSlots)
        {
            if (portraitSlot.idx == index)
            {
                Unit clickedUnit = portraitSlot.likedUnit;
                GameWorld.Instance.InputManager.SelectUnit(clickedUnit);
                break;
            }
        }

        UnitDetails();
    }

    public void SellUnits()
    {
        List<Unit> selectedUnits = GameWorld.Instance.UnitManager.selectedUnitList;
        for(int i = selectedUnits.Count - 1; i >= 0; --i)
        {
            Unit unit = selectedUnits[i];
            GameWorld.Instance.UnitManager.allUnitList.Remove(unit);
            GameWorld.Instance.playerGolds += unit.unitData.salesGold;
            Destroy(unit.gameObject);
        }
        selectedUnits.Clear();

        UnitDetails();
    }
}
