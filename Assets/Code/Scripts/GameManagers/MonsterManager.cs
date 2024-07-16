using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterManager : MonoBehaviour
{
    [Header("SelectedMonster"), Tooltip("몬스터 선택")]
    public Monster selectedMonster;
    public List<Monster> allMonsterList;

    [Header("Prefab"), Tooltip("몬스터 프리팹")]
    public GameObject[] monsterPrefabArray;
    [Header("Spawn"), Tooltip("몬스터 스폰지")]
    public Transform monsterSpawn;
    [Header("WayPoint"), Tooltip("몬스터 웨이포인트")]
    public Transform[] wayPointArray;

    private float _rotSpeed = 360.0f;

    //네브메쉬
    private NavMeshPath myPath;

    private void Awake()
    {
        selectedMonster = null;
        allMonsterList = new List<Monster>();
    }

    public void MonsterAI()
    {
        for (var i = allMonsterList.Count - 1; i >= 0; --i)
        {
            var monster = allMonsterList[i];
            if (monster.IsDead)
            {
                allMonsterList.RemoveAt(i);
                Destroy(monster.gameObject);
            }
        }
    }

    // 무브
    public void Move()
    {
        if (myPath == null) myPath = new NavMeshPath();
        foreach (Monster monster in allMonsterList)
        {
            if (monster.transform.position == wayPointArray[monster.curWayPointIdx].position)
            {
                monster.curWayPointIdx++;
                if (monster.curWayPointIdx > 3) monster.curWayPointIdx = 0;
            }
            if (NavMesh.CalculatePath(monster.transform.position, wayPointArray[monster.curWayPointIdx].position, NavMesh.AllAreas, myPath))
            {
                switch (myPath.status)
                {
                    case NavMeshPathStatus.PathComplete:
                    case NavMeshPathStatus.PathPartial:
                        if (myPath.corners.Length > 1)
                        {
                            var corner = myPath.corners[1];
                            Vector3 moveDir = corner - monster.transform.position;
                            float moveDist = moveDir.magnitude;
                            moveDir.Normalize();
                            float rotAngle = Vector3.Angle(monster.transform.forward, moveDir);

                            float rotDir = 1.0f;
                            if (Vector3.Dot(monster.transform.right, moveDir) < 0.0f) rotDir = -1.0f;

                            float rotateAmount = _rotSpeed * Time.deltaTime;
                            if (rotAngle < rotateAmount) rotateAmount = rotAngle;
                            monster.transform.Rotate(Vector3.up * rotDir * rotateAmount);

                            float moveAmount = monster.monsterData.Speed * Time.deltaTime;
                            if (moveDist < moveAmount) moveAmount = moveDist;
                            monster.transform.Translate(moveDir * moveAmount, Space.World);
                        }
                        break;
                    case NavMeshPathStatus.PathInvalid:
                        break;
                }
            }
        }
    }

    //스폰
    public void SpawnMonster()
    {
        GameObject obj = Instantiate(monsterPrefabArray[GameWorld.Instance.curRound - 1], monsterSpawn);
        Monster monster = obj.GetComponent<Monster>();

        int index = monster.name.IndexOf("(Clone)");
        string name = monster.name.Substring(0, index);

        var stat = new MonsterData()
        {
            Speed = 5.0f,
            HP = 1000,
        };
        monster.monsterData = stat;
        monster.Init();

        MonsterDB.instance.LoadMonsterStatFromXML(name, monster);

        allMonsterList.Add(monster);
    }

    // 몬스터 수 반환
    public int GetMonsterCount()
    {
        return allMonsterList.Count;
    }
}
