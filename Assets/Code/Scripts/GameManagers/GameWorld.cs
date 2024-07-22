using System;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using EasyUI.Toast;

public class GameWorld : Singleton<GameWorld>
{
    [SerializeField] private BalanceManager _balanceManager;
    [SerializeField] private UnitManager _unitManager;
    [SerializeField] private MonsterManager _monsterManager;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private BulletManager _bulletManager;
    [SerializeField] private FXManager _fxManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private SoundManager _soundManager;

    public BalanceManager BalanceManager => _balanceManager;
    public UnitManager UnitManager => _unitManager;
    public MonsterManager MonsterManager => _monsterManager;
    public InputManager InputManager => _inputManager;
    public BulletManager BulletManager => _bulletManager;
    public FXManager FXManager => _fxManager;
    public UIManager UIManager => _uiManager;
    public SoundManager SoundManager => _soundManager;


    private float spawnDelay;
    private float spawnInterval;
    private int spawnCount;

    public float normalRoundTime;
    public float bossRoundTime;
    public float roundElapsedTime;
    public float remainTime;

    public int curRound;
    public int totalRounds;

    public int playerGolds = 100;
    

    private void Awake()
    {
        playerGolds = 100;

        spawnDelay = 0.0f;
        spawnInterval = 1.0f;
        spawnCount = 0;

        normalRoundTime = 15.0f;
        bossRoundTime = 20.0f;
        roundElapsedTime = 0.0f;
        remainTime = 0.0f;

        curRound = 0;
        totalRounds = _monsterManager.monsterPrefabArray.Length;
    }

    private void Start()
    {
        _balanceManager.Init();

    }

    private void Update()
    {
/*        if (curRound < totalRounds)
        {
            float curRoundTime = (curRound % 5 == 0 && curRound != 0) ? bossRoundTime : normalRoundTime;
            int spawnMax = (curRound % 5 == 0 && curRound != 0) ? 1 : 5;
            if (curRound != 0 && spawnCount < spawnMax)
            {
                if (spawnDelay > spawnInterval)
                {
                    _monsterManager.SpawnMonster();
                    spawnCount++;
                    spawnDelay = 0.0f;
                }
                spawnDelay += Time.deltaTime;
            }
            roundElapsedTime += Time.deltaTime;
            remainTime = curRoundTime - roundElapsedTime;
            if (remainTime < 0.0f)
            {
                curRound++;
                roundElapsedTime = 0.0f;
                remainTime = 0.0f;
                spawnCount = 0;
            }
*//*            if (curRound >= totalRounds)
            {
                _uiManager.GameVictory();
            }

            if (_monsterManager.allMonsterList.Count >= 4)
            {
                _uiManager.GameOver();
            }*//*
        }*/

        _inputManager.AdvanceInput();
        if (spawnDelay > spawnInterval)
        {
            _monsterManager.SpawnMonster();
            spawnCount++;
            spawnDelay = 0.0f;
        }
        spawnDelay += Time.deltaTime;

        _unitManager.UnitAnim();

        _unitManager.UnitAI();
        _monsterManager.MonsterAI();
        _bulletManager.BulletAI();

        _unitManager.UnitAttack();

        _unitManager.UnitMove();
        _monsterManager.MonsterMove();
        _bulletManager.BulletMove();
    }
    public void TakeGold(int amount)
    {
        if (playerGolds >= amount)
        {
            playerGolds -= amount;
        }
        else
        {
            Toast.Show("골드가 부족합니다. <size=25> \n" + (amount - playerGolds).ToString() + " 골드가 더 필요합니다 </size> ", 2f, ToastColor.Magenta,ToastPosition.MiddleCenter);
        }
        if (playerGolds < 0) playerGolds = 0;
    }

}
