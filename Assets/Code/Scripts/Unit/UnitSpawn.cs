using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawn : MonoBehaviour
{
    public GameObject unitPrefab;
    public void SpawnUnit(){

        GameObject obj = Instantiate(unitPrefab, transform);
        Unit unit = obj.GetComponent<Unit>();
        
        Unit.allUnitList.Add(unit);
    }
}
