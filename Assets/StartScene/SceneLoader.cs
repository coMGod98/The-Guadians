using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public static int targetScene;
    public Slider mySlider;
    public GameObject Message;

    private TextMeshProUGUI myTMPText;
 
    
    // Start is called before the first frame update
    void Start()
    {
        myTMPText = Message.GetComponent<TextMeshProUGUI>();
        StartCoroutine(LoadingScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (mySlider.value==1.0f)
        {
            Message.SetActive(true);
            StartCoroutine(BlinkingTextAlpha());
        }
    }

    public static void LoadScene(int scene)
    {
        targetScene = scene;
        SceneManager.LoadScene(0);
    }

    IEnumerator LoadingScene()
    {
        mySlider.value = 0.0f;
        yield return new WaitForSeconds(0.5f);

        AsyncOperation ao = SceneManager.LoadSceneAsync(targetScene);
        ao.allowSceneActivation = false;
        
        while(mySlider.value < 1.0f)
        {
            yield return StartCoroutine(UpdatingSlider(ao.progress/0.9f));
        }

        while (true)
        {
            if (Input.anyKeyDown)
            {
                ao.allowSceneActivation = true;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator UpdatingSlider(float v)
    {
        while(mySlider.value < v)
        {
            mySlider.value += Time.deltaTime * 0.6f;
            yield return null;
        }
        mySlider.value = v;
    }

    IEnumerator BlinkingTextAlpha()
    {
        float alpha = 1.0f;
        bool fadingOut = true;
        Color originalColor = myTMPText.color;

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

            myTMPText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

    }
    }
