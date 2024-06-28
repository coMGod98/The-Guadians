using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Transform unitSpawn;
    public Transform monsterSpawn;
    public GameObject unitPrefab;
    public GameObject monsterPrefab;
    UnitController unitController;

    private void Start()
    {
        unitController = gameObject.AddComponent<UnitController>();
    }

    public void SpawnUnits(){

        GameObject obj = Instantiate(unitPrefab, unitSpawn);
        Unit unit = obj.GetComponent<Unit>();
        
        unitController.allUnitList.Add(unit);
    }
}
