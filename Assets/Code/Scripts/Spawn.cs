using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawn : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject[] unitPrefabArray;
    public GameObject[] monsterPrefabArray;
    public GameObject bossPrefab;

    [Header("Spawns")]
    public Transform unitSpawn;
    public Transform monsterSpawn;
    public Transform[] wayPointArray;

    [Header("Info")]
    public float spawnInterval = 2.0f;
    public int monsterSpawnCount = 0;
    private Coroutine spawningCoroutine;

    public void SpawnUnit()
    {
        int randIdx = Random.Range(0, unitPrefabArray.Length);
        Vector3 randomSpawn = RandomSpawn();
        GameObject obj = Instantiate(unitPrefabArray[randIdx], randomSpawn, Quaternion.identity);
        obj.transform.parent = unitSpawn;
        Unit unit = obj.GetComponent<Unit>();

        //allUnitList.Add(unit);
    }

    Vector3 RandomSpawn()
    {
        float radius = Random.Range(0.0f, 1.0f);
        float angle = Random.Range(0.0f, 360.0f);
        float x = radius * Mathf.Sin(angle);
        float z = radius * Mathf.Cos(angle);
        Vector3 randomVector = new Vector3(x, 0.55f, z);
        Vector3 randomPosition = transform.position + randomVector;
        return randomPosition;

    }

    public void SpawnMonster(int round)
    {
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
        }
        spawningCoroutine = StartCoroutine(SpawningMonster(round));
    }

    public void SpawnBoss()
    {
        GameObject obj = Instantiate(bossPrefab, monsterSpawn);
        Monster boss = obj.GetComponent<Monster>();
        Monster.allMonsterList.Add(boss);
        monsterSpawnCount++;
        boss.SetWaypoint(wayPointArray);
    }
    IEnumerator SpawningMonster(int round)
    {
        int monsterIndex = Mathf.Min(round - 1, monsterPrefabArray.Length - 1);
        while (true)
        {
            GameObject obj = Instantiate(monsterPrefabArray[monsterIndex], monsterSpawn.position, Quaternion.identity);
            Monster monster = obj.GetComponent<Monster>();
            Monster.allMonsterList.Add(monster);
            monster.SetWaypoint(wayPointArray);
            monsterSpawnCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}   