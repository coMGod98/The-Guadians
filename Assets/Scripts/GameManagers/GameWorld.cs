using UnityEngine;

public class GameWorld : Singleton<GameWorld>
{
    [SerializeField] private BalanceManager _balanceManager;
    [SerializeField] private UnitManager _unitManager;
    [SerializeField] private MonsterManager _monsterManager;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private BulletManager _bulletManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private AoeManager _aoeManager;
    [SerializeField] private SoundManager _soundManager;


    public BalanceManager BalanceManager => _balanceManager;
    public UnitManager UnitManager => _unitManager;
    public MonsterManager MonsterManager => _monsterManager;
    public InputManager InputManager => _inputManager;
    public BulletManager BulletManager => _bulletManager;
    public UIManager UIManager => _uiManager;
    public AoeManager AoeManager => _aoeManager;
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

    public int playerGolds;
    

    private void Awake()
    {
        playerGolds = 100;

        spawnDelay = 0.0f;
        spawnInterval = 1.0f;
        spawnCount = 0;

        normalRoundTime = 30.0f;
        bossRoundTime = 60.0f;
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
            curRoundTime = (curRound == 0) ? 20 : curRoundTime;
            int spawnMax = (curRound % 5 == 0 && curRound != 0) ? 1 : 30;
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

            if (curRound >= totalRounds && _monsterManager.allMonsterList.Count == 0)
            {
                _uiManager.gameWin.SetActive(true);
                _soundManager.background.Stop();
                Time.timeScale = 0.0f;
                
            }

            if (_monsterManager.allMonsterList.Count >= 10 || (curRound >= totalRounds && _monsterManager.allMonsterList.Count > 0))
            {
                _uiManager.gameLost.SetActive(true);
                _soundManager.background.Stop();
                Time.timeScale = 0.0f;
                
            }
        }

        _inputManager.AdvanceInput();

        spawnDelay += Time.deltaTime;

        _unitManager.UnitAnim();

        _unitManager.UnitAI();
        _monsterManager.MonsterAI();
        _bulletManager.BulletAttack();

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
       
        if (playerGolds < 0) playerGolds = 0;
    }

    public void AddGold(int amount) 
    {
        playerGolds += amount;
        _soundManager.goldAudio.PlayOneShot(_soundManager.gold);
    }
}
