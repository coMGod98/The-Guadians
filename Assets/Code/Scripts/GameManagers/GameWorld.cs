using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameWorld : Singleton<GameWorld>
{
    [SerializeField] private UnitManager _unitManager;
    [SerializeField] private MonsterManager _monsterManager;
    [SerializeField] private InputManager _inputManager;

    public UnitManager UnitManager => _unitManager;
    public MonsterManager MonsterManager => _monsterManager;
    public InputManager InputManager => _inputManager;


    private float spawnDelay;
    private float spawnInterval;
    private int spawnCount;

    private float normalRoundTime;
    private float bossRoundTime;
    private float roundElapsedTime;
    private float remainTime;

    public int curRound;
    private int totalRounds;

    private void Awake(){
        spawnDelay = 0.0f;
        spawnInterval = 1.0f;
        spawnCount = 0;

        normalRoundTime = 10.0f;
        bossRoundTime = 20.0f;
        roundElapsedTime = 0.0f;
        remainTime = 0.0f;

        curRound = 0;
        totalRounds = _monsterManager.monsterPrefabArray.Length;
    }
    private void Update()
    {
        if (curRound < totalRounds)
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
        }

        _inputManager.AdvanceInput();

        _unitManager.UnitAI();

        _unitManager.Move();
        _monsterManager.Move();
    }
}
