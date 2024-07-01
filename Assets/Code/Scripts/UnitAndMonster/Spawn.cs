using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Transform unitSpawn;
    public Transform monsterSpawn;
    public GameObject unitPrefab;
    public GameObject monsterPrefab;
    public Transform[] wayPoint;
    public float spawntime = 2.0f;

    void Start()
    {

    }

    public void SpawnUnits(){

        GameObject obj = Instantiate(unitPrefab, unitSpawn);
        Unit unit = obj.GetComponent<Unit>();
        
        UnitController.allUnitList.Add(unit);
    }

    public void SpawnMonsters()
    {
        GameObject obj = Instantiate(monsterPrefab, monsterSpawn);
        Monster monster = obj.GetComponent<Monster>();

        Monster.allMonsterList.Add(monster);
    }
}
