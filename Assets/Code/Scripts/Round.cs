using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Round : MonoBehaviour
{
    public UnityEvent act;
    float RoundTime = 0.0f;
    private void Update()
    {
        if (Mathf.Approximately(RoundTime, 0.0f))
        {
            act?.Invoke();
        }
        RoundTime += Time.deltaTime;
        if (RoundTime > 60) RoundTime = 0.0f;
    }
}
