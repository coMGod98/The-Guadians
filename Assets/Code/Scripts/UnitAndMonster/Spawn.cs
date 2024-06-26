using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Transform mySpawn;
    public GameObject unitPrefab;
    public GameObject monsterPrefab;
    public List<Unit> unitList = new List<Unit>();

    public void SpawnUnits(){

        GameObject obj = Instantiate(unitPrefab, mySpawn);
        Unit unit = obj.GetComponent<Unit>();
        
        unitList.Add(unit);
    }
}
