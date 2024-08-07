using UnityEngine;
using UnityEngine.EventSystems;

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

    public float m_DoubleClickSecond = 0.2f;
    private bool m_IsOneCkick = false;
    private double m_Timer = 0;


    private void Awake() {
        mainCamera = Camera.main;
        DrawDragRectangle();
    }

    public void AdvanceInput()
    {
        if(EventSystem.current.IsPointerOverGameObject() == false && GameWorld.Instance.UIManager.GameMenu.gameMenu.activeSelf == false)
        {
            if (m_IsOneCkick && ((Time.time - m_Timer) > m_DoubleClickSecond))
            {
                m_IsOneCkick = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                start = Input.mousePosition;
                dragRect = new Rect();
                if (!m_IsOneCkick)
                {
                    m_Timer = Time.time;
                    m_IsOneCkick = true;
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
                else if (m_IsOneCkick && ((Time.time - m_Timer) < m_DoubleClickSecond))
                {
                    m_IsOneCkick = false;

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitMask))
                    {
                        if (hit.transform.GetComponent<Unit>() == null) return;
                        DoubleSelectUnit(hit.transform.GetComponent<Unit>());
                    }
                    else
                    {
                        DeselectAll();
                    }
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
                DragSelectUnit();

                start = end = Vector2.zero;
                DrawDragRectangle();

                GameWorld.Instance.UIManager.ShowDetails.UnitDetails();
            }

            if (Input.GetMouseButtonDown(1)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);            
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, monsterMask)) {
                    Monster monster = hit.transform.GetComponent<Monster>();
                    GameWorld.Instance.UnitManager.InputTargeting(monster);
                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
                {
                    GameWorld.Instance.UnitManager.InputDestination(hit.point);
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            GameWorld.Instance.UnitManager.InputHold();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameWorld.Instance.UIManager.GameMenu.optionsMenu.activeSelf)
            {
                GameWorld.Instance.UIManager.GameMenu.optionsMenu.SetActive(false);                
            }
            else
            {
                GameWorld.Instance.UIManager.GameMenu.gameMenu.SetActive(!GameWorld.Instance.UIManager.GameMenu.gameMenu.activeSelf);
                if (GameWorld.Instance.UIManager.GameMenu.gameMenu.activeSelf)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1.0f;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitMask))
            {
                if (hit.transform.GetComponent<Unit>() == null) return;
                SameJobRankSelctUnit(hit.transform.GetComponent<Unit>());
            }
            else
            {
                DeselectAll();
            }
            GameWorld.Instance.UIManager.ShowDetails.UnitDetails();
        }
    }

    public void SelectUnit(Unit newUnit) {
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
        foreach(Monster monster in GameWorld.Instance.MonsterManager.allMonsterList)
        {
            monster.GetComponent<Outline>().enabled = false;
        }
        GameWorld.Instance.MonsterManager.selectedMonster = null;
    }

    public void DeselectAll() {
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

    private void DragSelectUnit() {
        foreach (Unit unit in GameWorld.Instance.UnitManager.allUnitList)
        {
            if (dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
            {
                if (!GameWorld.Instance.UnitManager.selectedUnitList.Contains(unit)) {
                    SelectUnit(unit);
                }
            }
        }
    }

    private void SameJobRankSelctUnit(Unit newUnit)
    {
        DeselectAll();
        foreach(Unit unit in GameWorld.Instance.UnitManager.allUnitList)
        {
            if(newUnit.unitData.job == unit.unitData.job && newUnit.unitData.rank == unit.unitData.rank)
            {
                SelectUnit(unit);
            }
        }
    }

    private void DoubleSelectUnit(Unit newUnit)
    {
        DeselectAll();
        foreach(Unit unit in GameWorld.Instance.UnitManager.allUnitList)
        {
            if(newUnit.unitData.job == unit.unitData.job)
            {
                SelectUnit(unit);
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