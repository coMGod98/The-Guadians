using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameWorld : Singleton<GameWorld>
{
    [SerializeField] private BalanceManager _balanceManager;
    [SerializeField] private UnitManager _unitManager;
    [SerializeField] private MonsterManager _monsterManager;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private BulletManager _bulletManager;
    [SerializeField] private UIManager _uiManager;

    public BalanceManager BalanceManager => _balanceManager;
    public UnitManager UnitManager => _unitManager;
    public MonsterManager MonsterManager => _monsterManager;
    public InputManager InputManager => _inputManager;
    public BulletManager BulletManager => _bulletManager;
    public UIManager UIManager => _uiManager;


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
            if (curRound >= totalRounds)
            {
                UIManager.Instance.GameVictory();
            }

            if (_monsterManager.allMonsterList.Count >= 4)
            {
                UIManager.Instance.GameOver();
            }
        }

        _inputManager.AdvanceInput();

        _unitManager.UnitAI();
        _monsterManager.MonsterAI();

        _unitManager.Move();
        _monsterManager.Move();
    }
    public void DeductGold(int amount)
    {
        playerGolds -= amount;
        if (playerGolds < 0) playerGolds = 0;
    }

}
