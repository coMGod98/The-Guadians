using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public List<GameObject> unitPrefabList = new List<GameObject>();
    public List<GameObject> monsterPrefabList = new List<GameObject>();
    public Transform unitSpawn;
    public Transform monsterSpawn;
    public Transform[] wayPointList;
    public float spawnInterval = 2.0f;


    public void SpawnUnit()
    {
        int randIdx = Random.Range(0, unitPrefabList.Count);
        GameObject obj = Instantiate(unitPrefabList[randIdx], unitSpawn);
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
            GameObject obj = Instantiate(monsterPrefabList[0], monsterSpawn);
            Monster monster = obj.GetComponent<Monster>();
            Monster.allMonsterList.Add(monster);
            monster.SetWaypoint(wayPointList);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
