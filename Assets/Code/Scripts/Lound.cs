using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lound : MonoBehaviour
{
    public UnityEvent startAct;
    // Start is called before the first frame update
    void Start()
    {
        startAct?.Invoke();
    }

}
