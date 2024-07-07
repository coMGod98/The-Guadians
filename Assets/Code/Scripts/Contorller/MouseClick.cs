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
    public LayerMask monsterMask;
    public UnityEvent<Monster> selectMonsterAct;
    public UnityEvent<Unit> oneUnitSelectAct;
    public UnityEvent emptySelecAct;
    public UnityEvent<Unit> mulUnitSelectAct;
    public UnityEvent<Vector3> moveAct;
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitMask)){
                if (hit.transform.GetComponent<Unit>() == null) return;
                if(Input.GetKey(KeyCode.LeftControl)) mulUnitSelectAct?.Invoke(hit.transform.GetComponent<Unit>());
                else oneUnitSelectAct?.Invoke(hit.transform.GetComponent<Unit>());
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, monsterMask)){
                if(hit.transform.GetComponent<Monster>() == null) return;
                selectMonsterAct?.Invoke(hit.transform.GetComponent<Monster>());
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
