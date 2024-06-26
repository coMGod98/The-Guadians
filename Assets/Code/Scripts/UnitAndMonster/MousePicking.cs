using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MousePicking : MonoBehaviour
{
    public LayerMask groundMask;
    public LayerMask unitMask;
    public UnityEvent<Vector3> moveAct;

    void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitMask))
            {
                //attackAct?.Invoke(hit.transform);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
            {
                //if(clickAct != null) clickAct.Invoke(hit.point);
                moveAct?.Invoke(hit.point);
            }
        }
    }
}
