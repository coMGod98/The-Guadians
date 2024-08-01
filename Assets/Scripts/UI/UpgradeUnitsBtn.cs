using System.Collections.Generic;
using UnityEngine;
using EasyUI.Toast;
using UnityEngine.UI;
public class UpgradeUnitsBtn : MonoBehaviour
{
    public List<int> costWarriorUpgrade;
    public List<int> costArcherUpgrade;
    public List<int> costWizardUpgrade;

    private void Awake()
    {
        costWarriorUpgrade = new List<int>();
        costArcherUpgrade = new List<int>();
        costWizardUpgrade = new List<int>();
        for(int i = 0; i < 20; i++)
        {
            costWarriorUpgrade.Add(5 + 5 * i);
            costArcherUpgrade.Add(5 + 5 * i);
            costWizardUpgrade.Add(5 + 5 * i);
        }
    }

    public void UnitsUpgradeBtn(UnitJob job, int cost)
    {
        if(GameWorld.Instance.UIManager.isButtonLocked)
        {
            Toast.Show("Skill Not Used Yet", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
            return;
        }
        else
        {
            if (GameWorld.Instance.playerGolds < cost)
            {
                GameWorld.Instance.UIManager.Alert(cost, GameWorld.Instance.playerGolds);
            }
            else
            {
                GameWorld.Instance.TakeGold(cost);
                switch(job)
                {
                    case UnitJob.Warrior:
                    {
                        GameWorld.Instance.UnitManager.warriorUpgrade++;
                        break;
                    }
                    case UnitJob.Archer:
                    {
                        GameWorld.Instance.UnitManager.archerUpgrade++;
                        break;
                    }
                    case UnitJob.Wizard:
                    {
                        GameWorld.Instance.UnitManager.wizardUpgrade++;
                        break;
                    }
                }        
            }
        }
        GameWorld.Instance.UIManager.ShowDetails.UnitDetails();
    }

    public void WarriorUpgradeBtn() => UnitsUpgradeBtn(UnitJob.Warrior, costWarriorUpgrade[GameWorld.Instance.UnitManager.warriorUpgrade]);
    public void ArcherUpgradeBtn() => UnitsUpgradeBtn(UnitJob.Archer, costArcherUpgrade[GameWorld.Instance.UnitManager.archerUpgrade]);
    public void WizardUpgradeBtn() => UnitsUpgradeBtn(UnitJob.Wizard, costWizardUpgrade[GameWorld.Instance.UnitManager.wizardUpgrade]);
}
