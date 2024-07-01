using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform[] wayPointList;
    public float spawnInterval = 2.0f;


    void Start(){
        StartCoroutine(SpawnMonster());
    }

    IEnumerator SpawnMonster()
    {
        while(true){
            GameObject obj = Instantiate(monsterPrefab, transform);
            Monster monster = obj.GetComponent<Monster>();
            Monster.allMonsterList.Add(monster);
            monster.SetWaypoint(wayPointList);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
