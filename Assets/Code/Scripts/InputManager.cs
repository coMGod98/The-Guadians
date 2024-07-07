using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{

    [Header("LayerMask")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask unitMask;
    [SerializeField] private LayerMask monsterMask;
    [Header("UnitSelect")]
    [SerializeField] List<Unit> allUnitList = UnitManager.allUnitList;
    [SerializeField] List<Unit> selectUnitList = UnitManager.selectUnitList;
    [Header("MonsetrSelect")]
    [SerializeField] private Monster selectMonster = null;
    [Header("Drag")]
    [SerializeField] private RectTransform dragRectangle;
    [SerializeField] private Vector2 start = Vector2.zero;
    [SerializeField] private Vector2 end = Vector2.zero;

    Rect dragRect;
    Camera mainCamera;

    [Header("UnitManager")]
    public UnityEvent<Vector3> moveAct;
    

    private void Awake() {
        mainCamera = Camera.main;
		DrawDragRectangle();
    }

    public void AdvanceInput()
    {
        if (Input.GetMouseButtonDown(0)){
            start = Input.mousePosition;
            dragRect = new Rect();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitMask)){
                if (hit.transform.GetComponent<Unit>() == null) return;
                if(Input.GetKey(KeyCode.LeftControl)) MulSelectUnit(hit.transform.GetComponent<Unit>());
                else OneSelectUnit(hit.transform.GetComponent<Unit>());
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, monsterMask)){
                if(hit.transform.GetComponent<Monster>() == null) return;
                SelectMonster(hit.transform.GetComponent<Monster>());
            }
            else
            {
                DeselectAll();
            }
        }

        if ( Input.GetMouseButton(0) )
		{
			end = Input.mousePosition;
			DrawDragRectangle();
		}

		if ( Input.GetMouseButtonUp(0) )
		{
			CalculateDragRect();
			DragUnit();

			start = end = Vector2.zero;
			DrawDragRectangle();
		}

        if(Input.GetMouseButtonDown(1)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask)){
                MoveSelectUnit(hit.point);
            }
        }
    }

    void SelectUnit(Unit newUnit){
        newUnit.unitMarker.SetActive(true);
        selectUnitList.Add(newUnit);
    }

    void DeselectUnit(Unit newUnit){
        newUnit.unitMarker.SetActive(false);
        selectUnitList.Remove(newUnit);
    }

    public void SelectMonster(Monster newMonster){
        DeselectAll();
        newMonster.GetComponent<Outline>().enabled = true;
        selectMonster = newMonster;
    }

    public void DeselectMonster(){
        if(selectMonster != null){
            selectMonster.GetComponent<Outline>().enabled = false;
            selectMonster = null;
        }
    }

    public void DeselectAll(){
        if (selectUnitList.Count > 0)
        {
            for (int i = 0; i < selectUnitList.Count; i++)
            {
                DeselectUnit(selectUnitList[i]);
            }
            selectUnitList.Clear();
        }
        DeselectMonster();
    }

    public void OneSelectUnit(Unit newUnit){
        DeselectAll();
        SelectUnit(newUnit);
    }

    public void MulSelectUnit(Unit newUnit){
        if(selectUnitList.Contains(newUnit)) DeselectUnit(newUnit);
        else SelectUnit(newUnit);
    }

    public void DragSelectUnit(Unit newUnit){
        foreach (Unit unit in allUnitList)
		{
			if ( dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)) )
			{
                if ( !selectUnitList.Contains(newUnit) ){
			        SelectUnit(newUnit);
		        }
			}
		}
    }

	void DragUnit()
	{
		foreach (Unit unit in allUnitList)
		{
			if ( dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)) )
			{
                DragSelectUnit(unit);
			}
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

    public void MoveSelectUnit(Vector3 pos){
        moveAct?.Invoke(pos);
    }
}
