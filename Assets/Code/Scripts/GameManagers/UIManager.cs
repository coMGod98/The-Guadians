using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Info")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI monsterCountText;

    [Header("Panel")]
    public GameObject gameWinPanel;
    public GameObject gameLostPanel;

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

    }
}
