using EasyUI.Toast;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
    }

    public void Update()
    {
        if (isPlacingAOE)
        {
            Range();
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

    public void Range()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int placeableMask = LayerMask.GetMask("AOE");
        int unplaceableMask = LayerMask.GetMask("AOE") | LayerMask.GetMask("Ground") | LayerMask.GetMask("Water");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placeableMask))
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

        int placeableMask = LayerMask.GetMask("AOE");
        int unplaceableMask = LayerMask.GetMask("AOE") | LayerMask.GetMask("Ground") | LayerMask.GetMask("Water");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placeableMask))
        {
            Vector3 towerPosition = hit.point;
            Instantiate(aoePrefabs[aoeIndex], towerPosition, Quaternion.identity);
            isPlacingAOE = false;
            curRangeCheck.SetActive(false);
            curRangeCheckX.SetActive(false);
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, unplaceableMask))
        {
            Toast.Show("설치 불가능", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
        }
    }

    public void SelectedAOE(int index) 
    {
        if (index >= 0 && index < aoePrefabs.Length)
        {
            aoeIndex = index;
        }
    }

    public void OnAoeButtonClicked(int index)
    {
        SelectedAOE(index); 
        ButtonClicked();    
    }

}

