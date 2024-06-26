using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseDrag : MonoBehaviour
{
    Spawn list;
    Vector2 start = Vector2.zero;
    Vector2 end = Vector2.zero;
    Camera mainCamera;
    Rect dragRect;

    [SerializeField]
    RectTransform dragRectangle;
    public UnityEvent<Unit> dragSelectAct;

    private void Awake() {
        mainCamera = Camera.main;
    }
    void Update(){
        if ( Input.GetMouseButtonDown(0) )
		{
			start = Input.mousePosition;
			dragRect = new Rect();
		}
		
		if ( Input.GetMouseButton(0) )
		{
			end = Input.mousePosition;
			DrawDragRectangle();
		}

		if ( Input.GetMouseButtonUp(0) )
		{
			CalculateDragRect();
			SelectUnits();

			start = end = Vector2.zero;
			DrawDragRectangle();
		}
    }

    void DrawDragRectangle(){
        dragRectangle.position	= (start + end) * 0.5f;
		dragRectangle.sizeDelta	= new Vector2(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));        
    }

	void CalculateDragRect()
	{
		if ( Input.mousePosition.x < start.x )
		{
			dragRect.xMin = Input.mousePosition.x;
			dragRect.xMax = start.x;
		}
		else
		{
			dragRect.xMin = start.x;
			dragRect.xMax = Input.mousePosition.x;
		}

		if ( Input.mousePosition.y < start.y )
		{
			dragRect.yMin = Input.mousePosition.y;
			dragRect.yMax = start.y;
		}
		else
		{
			dragRect.yMin = start.y;
			dragRect.yMax = Input.mousePosition.y;
		}
	}

	void SelectUnits()
	{
		foreach (Unit unit in list.unitList)
		{
			if ( dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)) )
			{
                dragSelectAct?.Invoke(unit);
			}
		}
	}
}
