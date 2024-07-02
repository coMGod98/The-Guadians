using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject unitPrefab;
    public GameObject monsterPrefab;
    public Transform unitSpawn;
    public Transform monsterSpawn;
    public Transform[] wayPointList;
    public float spawnInterval = 2.0f;


    public void SpawnUnit()
    {
        GameObject obj = Instantiate(unitPrefab, unitSpawn);
        Unit unit = obj.GetComponent<Unit>();

        Unit.allUnitList.Add(unit);
    }

    public void SpawnMonster()
    {
        StartCoroutine(SpawningMonster());
    }

    IEnumerator SpawningMonster()
    {
        while (true)
        {
            GameObject obj = Instantiate(monsterPrefab, monsterSpawn);
            Monster monster = obj.GetComponent<Monster>();
            Monster.allMonsterList.Add(monster);
            monster.SetWaypoint(wayPointList);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
