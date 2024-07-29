using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static int targetScene;
    public Slider mySlider;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingScene());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        {
            //SceneManager.LoadScene(targetScene);
            //SceneManager.LoadSceneAsync(targetScene);
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
}
