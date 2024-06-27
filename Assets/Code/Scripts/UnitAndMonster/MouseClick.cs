using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MouseClick : MonoBehaviour
{
    public LayerMask unitMask;
    public LayerMask groundMask;
    public LayerMask attackMask;
    public UnityEvent emptySelecAct;
    public UnityEvent<Unit> oneSelectAct;
    public UnityEvent<Unit> mulSelectAct;
    public UnityEvent<Vector3> moveAct;
    

    void Update()
    {

        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitMask)){
                if (hit.transform.GetComponent<Unit>() == null) return;
                if(Input.GetKey(KeyCode.LeftControl)) mulSelectAct?.Invoke(hit.transform.GetComponent<Unit>());
                else oneSelectAct?.Invoke(hit.transform.GetComponent<Unit>());
            }
            else
            {
                emptySelecAct?.Invoke();
            }
        }

        if(Input.GetMouseButtonDown(1)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask)){
                moveAct?.Invoke(hit.point);
            }
        }
    }
    
}
