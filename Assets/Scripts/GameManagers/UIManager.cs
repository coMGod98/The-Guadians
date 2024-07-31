using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using EasyUI.Toast;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] ShowDetails _showDetails;
    [SerializeField] GameMenu _gameMenu;
    [SerializeField] AoeBtn _aodeBtn;
    [SerializeField] UpgradeUnitsBtn _upgradeUnitsBtn;
    [SerializeField] SpawnUnitsBtn _spawnUnitsBtn;
    
    public ShowDetails ShowDetails => _showDetails;
    public GameMenu GameMenu => _gameMenu;
    public AoeBtn AoeBtn => _aodeBtn;
    public UpgradeUnitsBtn UpgradeUnits => _upgradeUnitsBtn;
    public SpawnUnitsBtn SpawnUnitsBtn => _spawnUnitsBtn;
    
    public bool isButtonLocked = false;


    [Header("Info")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI monsterCountText;
    public TextMeshProUGUI curGold;

    [Header("Panel")]
    public GameObject gameWin;
    public GameObject gameLost;

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

    public void GameState(bool isState)
    {
        gameWin.SetActive(isState);
        gameLost.SetActive(!isState);
    }

    public void Alert(int cost, int playerGold)
    {
        int neededGold = cost - GameWorld.Instance.playerGolds;
        Toast.Show($"Not Enough Gold. <size=25> \n{neededGold} Needed gold : </size>", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
    }

    // 치팅
    public void addGold()
    {
        GameWorld.Instance.AddGold(100);
    }
}
