using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class Pannel : MonoBehaviour
{
    public GameObject Victory;
    public GameObject PressanyKey;

    private TextMeshProUGUI Vic;
    private TextMeshProUGUI PaK;

    public float fadeDuration = 2.0f;
    public float delay = 1.0f;

    private bool isBlinking = false;
    private bool isFadein = false;

    void Start()
    {
        Vic = Victory.GetComponent<TextMeshProUGUI>();
        PaK = PressanyKey.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (PressanyKey.activeSelf && !isBlinking)
        {
            StartCoroutine(BlinkingTextAlpha());
        }

        if (Victory.activeSelf && !isFadein)
        {
            StartCoroutine(FadeIn());
        }

        if (Input.anyKeyDown)
        {
            SceneLoader.LoadScene(1);
            Time.timeScale = 1.0f;
        }
    }

    IEnumerator BlinkingTextAlpha()
    {
        isBlinking = true;
        float alpha = 1.0f;
        bool fadingOut = true;
        Color originalColor = PaK.color;

        while (PressanyKey.activeSelf)
        {
            if (fadingOut)
            {
                alpha -= Time.deltaTime;
                if (alpha <= 0)
                {
                    alpha = 0;
                    fadingOut = false;
                }
            }
            else
            {
                alpha += Time.deltaTime;
                if (alpha >= 1.0f)
                {
                    alpha = 1.0f;
                    fadingOut = true;
                }
            }

            PaK.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 패널이 비활성화되면 원래 알파값으로 복원하고 코루틴 종료
        PaK.color = originalColor;
        isBlinking = false;
    }

    IEnumerator FadeIn()
    {
        isFadein = true;
        yield return new WaitForSeconds(delay);
        Color color = Vic.color;
        float elapsedTime = 0f;

        // 알파값을 0으로 설정
        color.a = 0f;
        Vic.color = color;

        while (color.a < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration); // 알파값을 0에서 1로 증가
            Vic.color = color;
            yield return null;
        }
        color.a = 1.0f;
        Vic.color = color;

        isFadein = false;
    }
}
