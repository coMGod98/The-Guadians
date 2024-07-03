using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform[] wayPointList;
    public float spawnInterval = 2.0f;
    public TextMeshProUGUI monsterCountText;

    void Start()
    {
        StartCoroutine(SpawnMonster());
        MonsterCounter(); 
    }

    IEnumerator SpawnMonster()
    {
        while (true)
        {
            GameObject obj = Instantiate(monsterPrefab, transform);
            Monster monster = obj.GetComponent<Monster>();
            Monster.allMonsterList.Add(monster);
            monster.SetWaypoint(wayPointList);
            MonsterCounter(); 
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void MonsterCounter()  
    {
        monsterCountText.text = "" + Monster.allMonsterList.Count;
    }
}
