using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundSystem : MonoBehaviour
{
    public TextMeshProUGUI monsterCountText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI roundTimeText;
    public GameObject gameOver;
    public GameObject gameComplete;
    public float Round;
    public float Boss;

    private int currentRound = 1;
    private int totalRounds = 20;
    private bool isGameOver = false;
    private float roundTime;

    void Start()
    {
        StartCoroutine(Round());
    }

    void Update()
    {
        if (isGameOver) return;
        int monsterCount = int.Parse(monsterCountText.text);
        if (monsterCount > 300)
        {
            GameOver();
        }

        roundTime -= Time.deltaTime;
        roundTimeText.text = Mathf.Ceil(roundTime).ToString();
    }

    IEnumerator Round()
    {
        while (currentRound <= totalRounds && !isGameOver)
        {
            UpdateRoundText();
            roundTime = currentRound % 5 == 0 ? Boss : round;

            float elapsedTime = 0f;
            while (elapsedTime < roundTime && !isGameOver)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentRound++;
        }

        if (currentRound > totalRounds && !isGameOver)
        {
            GameComplete();
        }
    }

    void UpdateRoundText()
    {
        roundText.text = currentRound.ToString();
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        gameOver.SetActive(true);
    }

    void GameComplete()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        gameComplete.SetActive(true);
    }
}
