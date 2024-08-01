using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeTooltip : MonoBehaviour
{
    public GameObject tooltipUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI goldText;

    public void SetUpTooltip(string name, int upgrade, int gold)
    {
        nameText.text = name;
        infoText. text = $"{name} {upgrade + 1} enhance";
        goldText.text = $"cost: {gold}";
    }

    public void TooltipUpdate()
    {
        tooltipUI.transform.position = Input.mousePosition;
    }

}
