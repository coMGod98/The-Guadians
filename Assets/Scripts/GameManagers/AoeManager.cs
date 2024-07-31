using EasyUI.Toast;
using UnityEngine;
using UnityEngine.UI;
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
    private int unplaceableMask;

    public event Action AoePlaced;

    void Start()
    {
        placeableMask = LayerMask.GetMask("Ground");
        unplaceableMask = LayerMask.GetMask("Water") | LayerMask.GetMask("Grounds") ;
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
            Vector3 indicatorPosition = hit.point;
            curRangeCheck.transform.position = indicatorPosition;
            curRangeCheck.SetActive(true);
            curRangeCheckX.SetActive(false);
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, unplaceableMask))
        {
            Vector3 indicatorPosition = hit.point;
            curRangeCheckX.transform.position = indicatorPosition;
            curRangeCheckX.SetActive(true);
            curRangeCheck.SetActive(false);
        }
    }

    public void PlaceAOE()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placeableMask) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Vector3 towerPosition = hit.point;
            Instantiate(aoePrefabs[aoeIndex], towerPosition, Quaternion.identity);
            isPlacingAOE = false;
            curRangeCheck.SetActive(false);
            curRangeCheckX.SetActive(false);
            AoePlaced?.Invoke();
            GameWorld.Instance.UIManager.isButtonLocked = false;
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, unplaceableMask))
        {
            Toast.Show("��ġ �Ұ���", 2f, ToastColor.Black, ToastPosition.MiddleCenter); // ��ġ�� �Ұ��� �Ҷ� ������ �佺Ʈ �޽���
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
