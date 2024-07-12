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

    [Header("Drag")]
    [SerializeField] private RectTransform dragRectangle;
    [SerializeField] private Vector2 start = Vector2.zero;
    [SerializeField] private Vector2 end = Vector2.zero;

    Rect dragRect;
    Camera mainCamera;


    private void Awake() {
        mainCamera = Camera.main;
        DrawDragRectangle();
    }

    public void AdvanceInput()
    {
        if (Input.GetMouseButtonDown(0)) {
            start = Input.mousePosition;
            dragRect = new Rect();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitMask)) {
                if (hit.transform.GetComponent<Unit>() == null) return;
                if (Input.GetKey(KeyCode.LeftControl)) MulSelectUnit(hit.transform.GetComponent<Unit>());
                else OneSelectUnit(hit.transform.GetComponent<Unit>());
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, monsterMask)) {
                if (hit.transform.GetComponent<Monster>() == null) return;
                SelectMonster(hit.transform.GetComponent<Monster>());
            }
            else
            {
                DeselectAll();
            }
        }

        if (Input.GetMouseButton(0))
        {
            end = Input.mousePosition;
            DrawDragRectangle();
        }

        if (Input.GetMouseButtonUp(0))
        {
            CalculateDragRect();
            DragUnit();

            start = end = Vector2.zero;
            DrawDragRectangle();
        }

        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask)) {
                GameWorld.Instance.UnitManager.SetDesinationdUnits(hit.point);
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, monsterMask)) {
                GameWorld.Instance.UnitManager.SetDesinationdUnits(hit.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            GameWorld.Instance.UnitManager.OnHold();
        }
    }

    private void SelectUnit(Unit newUnit) {
        newUnit.unitMarker.SetActive(true);
        GameWorld.Instance.UnitManager.selectedUnitList.Add(newUnit);
    }

    private void DeselectUnit(Unit newUnit) {
        newUnit.unitMarker.SetActive(false);
        GameWorld.Instance.UnitManager.selectedUnitList.Remove(newUnit);
    }

    private void SelectMonster(Monster newMonster) {
        DeselectAll();
        newMonster.GetComponent<Outline>().enabled = true;
        GameWorld.Instance.MonsterManager.selectedMonster = newMonster;
    }

    private void DeselectMonster() {
        if (GameWorld.Instance.MonsterManager.selectedMonster != null) {
            GameWorld.Instance.MonsterManager.selectedMonster.GetComponent<Outline>().enabled = false;
            GameWorld.Instance.MonsterManager.selectedMonster = null;
        }
    }

    private void DeselectAll() {
        foreach (Unit unit in GameWorld.Instance.UnitManager.selectedUnitList)
        {
            unit.unitMarker.SetActive(false);
        }
        GameWorld.Instance.UnitManager.selectedUnitList.Clear();
        DeselectMonster();
    }

    private void OneSelectUnit(Unit newUnit) {
        DeselectAll();
        SelectUnit(newUnit);
    }

    private void MulSelectUnit(Unit newUnit) {
        if (GameWorld.Instance.UnitManager.selectedUnitList.Contains(newUnit)) DeselectUnit(newUnit);
        else SelectUnit(newUnit);
    }

    private void DragSelectUnit(Unit newUnit) {
        foreach (Unit unit in GameWorld.Instance.UnitManager.allUnitList)
        {
            if (dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
            {
                if (!GameWorld.Instance.UnitManager.selectedUnitList.Contains(newUnit)) {
                    SelectUnit(newUnit);
                }
            }
        }
    }

    private void DragUnit()
    {
        foreach (Unit unit in GameWorld.Instance.UnitManager.allUnitList)
        {
            if (dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
            {
                DragSelectUnit(unit);
            }
        }
    }

    private void DrawDragRectangle() {
        dragRectangle.position = (start + end) * 0.5f;
        dragRectangle.sizeDelta = new Vector2(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
    }

    private void CalculateDragRect()
    {
        if (Input.mousePosition.x < start.x)
        {
            dragRect.xMin = Input.mousePosition.x;
            dragRect.xMax = start.x;
        }
        else
        {
            dragRect.xMin = start.x;
            dragRect.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < start.y)
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
}