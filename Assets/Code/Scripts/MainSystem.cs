using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MainSystem : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI monsterCountText;

    [Header("Panel")]
    public GameObject gameWinPanel;
    public GameObject gameLostPanel;

    [Header("GameInfo")]
    public float normalRound = 60.0f; 
    public float bossRound = 120.0f;
    public int totalRounds = 20;

    [Header("Scripts")]
    //public Spawn spawnScript; 

    private float RoundTime = 0.0f;
    private int currentRound = 1;

    private void Start()
    {
/*        if (spawnScript == null)
        {
            spawnScript = GetComponent<Spawn>();
        }*/
    }

    private void Update()
    {
        float roundDuration = (currentRound % 5 == 0) ? bossRound : normalRound;

        if (Mathf.Approximately(RoundTime, 0.0f))
        {
            if (currentRound % 5 == 0)
            {
                GameWorld?.Curro
                //spawnScript?.SpawnBoss();
            }
            else
            {
                //spawnScript?.SpawnMonster(currentRound);
            }
        }
        RoundTime += Time.deltaTime;

        
        float remainingTime = roundDuration - RoundTime;
        if (remainingTime < 0) remainingTime = 0;

       
        if (timerText != null)
        {
            timerText.text = $"{remainingTime:F2}";
        }

        
        if (roundText != null)
        {
            roundText.text = $"{currentRound}/{totalRounds}";
        }

        
        if (RoundTime > roundDuration)
        {
            RoundTime = 0.0f;
            currentRound++;
        }
    }
}
