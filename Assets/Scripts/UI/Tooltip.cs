using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public GameObject tooltipUI;
    public TextMeshProUGUI jobText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI goldText;

    public void SetUpTooltip(string job, int upgrade, int gold)
    {
        jobText.text = job;
        infoText. text = $"{job} {upgrade + 1} enhance";
        goldText.text = $"cost: {gold}";
    }

    public void TooltipUpdate()
    {
        tooltipUI.transform.position = Input.mousePosition;
    }

}
