using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform spawnPoint;
    public Transform[] waypoints;
    public float spawntime = 2.0f;

    void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            GameObject monster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
            MonsterMovement movement = monster.GetComponent<MonsterMovement>();
            if (movement != null)
            {
                movement.SetWaypoints(waypoints);
            }
            yield return new WaitForSeconds(spawntime);
        }
    }
}
