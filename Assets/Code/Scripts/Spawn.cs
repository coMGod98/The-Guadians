using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        Vector3 randomSpawn = RandomSpawn();
        GameObject obj = Instantiate(unitPrefabArray[randIdx], randomSpawn, Quaternion.identity);
        obj.transform.parent = unitSpawn;
        Unit unit = obj.GetComponent<Unit>();

        Unit.allUnitList.Add(unit);
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
