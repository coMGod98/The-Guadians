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
    void Start()
    {
        Vic = Victory.GetComponent<TextMeshProUGUI>();
        PaK = PressanyKey.GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeIn());
    }
    void Update()
    {
        
        StartCoroutine(BlinkingTextAlpha());
        if (Input.anyKeyDown)
        {
            SceneLoader.LoadScene(1);
            Time.timeScale = 1.0f;
        }

    }
    IEnumerator BlinkingTextAlpha()
    {
        float alpha = 1.0f;
        bool fadingOut = true;
        Color originalColor = PaK.color;

        while (true)
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
    }

        IEnumerator FadeIn()
        {
        yield return new WaitForSeconds(delay);
        Color color = Vic.color;
            float elapsedTime = 0f;

            // 알파값을 0으로 설정
            color.a = 0f;
            Vic.color = color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration); // 알파값을 0에서 1로 증가
                Vic.color = color;
                yield return null;
            }

        
    }
    }

