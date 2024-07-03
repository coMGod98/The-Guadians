using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject[] unitPrefabArray;
    public GameObject[] monsterPrefabArray;
    public Transform unitSpawn;
    public Transform monsterSpawn;
    public Transform[] wayPointArray;
    public float spawnInterval = 2.0f;


    public void SpawnUnit()
    {
        int randIdx = Random.Range(0, unitPrefabArray.Length);
        GameObject obj = Instantiate(unitPrefabArray[randIdx], unitSpawn);
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
            GameObject obj = Instantiate(monsterPrefabArray[0], monsterSpawn);
            Monster monster = obj.GetComponent<Monster>();
            Monster.allMonsterList.Add(monster);
            monster.SetWaypoint(wayPointArray);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
