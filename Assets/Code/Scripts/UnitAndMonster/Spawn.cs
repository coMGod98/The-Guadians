using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Transform unitSpawn;
    public Transform monsterSpawn;
    public GameObject unitPrefab;
    public GameObject monsterPrefab;

    public void SpawnUnits(){

        GameObject obj = Instantiate(unitPrefab, unitSpawn);
        Unit unit = obj.GetComponent<Unit>();
        
        UnitController.allUnitList.Add(unit);
    }
}
