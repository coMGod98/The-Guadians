using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeTooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UpgradeTooltip tooltip;
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.tooltipUI.SetActive(true);
        ChangeTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.tooltipUI.SetActive(false);
    }

    public void ChangeTooltip()
    {
        UnitJob job = (UnitJob)System.Enum.Parse(typeof(UnitJob), GetComponent<UpgradeTooltipController>().gameObject.name);
        switch(job)
        {
            case UnitJob.Warrior:
                {
                    if(GameWorld.Instance.UnitManager.warriorUpgrade > 18)
                    {
                        tooltip.SetUpTooltip(job.ToString());
                    }
                    else
                    {
                        tooltip.SetUpTooltip(job.ToString(), GameWorld.Instance.UnitManager.warriorUpgrade, 
                        GameWorld.Instance.UIManager.UpgradeUnitsBtn.costWarriorUpgrade[GameWorld.Instance.UnitManager.warriorUpgrade]);
                    }
                    break;
                }
            case UnitJob.Archer:
            {
                if(GameWorld.Instance.UnitManager.archerUpgrade > 18)
                {
                    tooltip.SetUpTooltip(job.ToString());
                }
                else
                {
                    tooltip.SetUpTooltip(job.ToString(), GameWorld.Instance.UnitManager.archerUpgrade, 
                    GameWorld.Instance.UIManager.UpgradeUnitsBtn.costArcherUpgrade[GameWorld.Instance.UnitManager.archerUpgrade]);
                }
                break;
            }
            case UnitJob.Wizard:
            {
                if(GameWorld.Instance.UnitManager.wizardUpgrade > 18)
                {
                    tooltip.SetUpTooltip(job.ToString());
                }
                else
                {
                    tooltip.SetUpTooltip(job.ToString(), GameWorld.Instance.UnitManager.wizardUpgrade, 
                    GameWorld.Instance.UIManager.UpgradeUnitsBtn.costWizardUpgrade[GameWorld.Instance.UnitManager.wizardUpgrade]);
                }
                break;
            }
        }
    }
}
