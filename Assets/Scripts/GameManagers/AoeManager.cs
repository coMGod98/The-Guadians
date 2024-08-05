using EasyUI.Toast;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class AoeManager : MonoBehaviour
{
    [Header("aoe Prefab")]
    public GameObject[] aoePrefabs;

    [Header("range Prefab")]
    public GameObject rangePrefab;
    public GameObject rangePrefabX;

    private GameObject curRangeCheck;
    private GameObject curRangeCheckX;

    private bool isPlacingAOE = false;
    private int aoeIndex = 0;
    private int placeableMask;

    public event Action AoePlaced;

    void Start()
    {
        placeableMask = LayerMask.GetMask("Ground");
    }
    public void Update()
    {
        if (isPlacingAOE)
        {
            RangeofAOE();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceAOE();
            }
        }
    }

    public void ButtonClicked()
    {
        isPlacingAOE = true;
        if (curRangeCheck == null)
        {
            curRangeCheck = Instantiate(rangePrefab);
        }
        if (curRangeCheckX == null)
        {
            curRangeCheckX = Instantiate(rangePrefabX);
        }
        curRangeCheck.SetActive(true);
        curRangeCheckX.SetActive(false);
    }

    public void RangeofAOE()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placeableMask) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Vector3 indicatorPosition = new Vector3(hit.point.x, 0.7f, hit.point.z);
            curRangeCheck.transform.position = indicatorPosition;
            curRangeCheck.SetActive(true);
            curRangeCheckX.SetActive(false);
        }
    }

    public void PlaceAOE()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placeableMask) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Vector3 towerPosition = new Vector3(hit.point.x, 0.7f, hit.point.z);
            Instantiate(aoePrefabs[aoeIndex], towerPosition, Quaternion.identity);
            isPlacingAOE = false;
            curRangeCheck.SetActive(false);
            curRangeCheckX.SetActive(false);
            AoePlaced?.Invoke();
            GameWorld.Instance.UIManager.isButtonLocked = false;
        }
    }

    public void SelectedAOE(int index)
    {
        if (index >= 0 && index < aoePrefabs.Length)
        {
            aoeIndex = index;
        }
    }

    public void ButtonClick(int index)
    {
        SelectedAOE(index);
        ButtonClicked();
    }
}
