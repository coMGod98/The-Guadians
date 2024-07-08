using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterManager : MonoBehaviour
{
    [Header("SelectedMonster"), Tooltip("몬스터 선택")]
    public Monster selectedMonster;
    public List<Monster> allMonsterList;

    [Header("Prefab"), Tooltip("몬스터 프리팹")]
    public GameObject[] monsterPrefabArray;
    public GameObject bossPrefab;
    [Header("Spawn"), Tooltip("몬스터 스폰지")]
    public Transform monsterSpawn;
    [Header("SpawnInfo"), Tooltip("몬스터 스폰 정보")]
    public float spawnInterval = 2.0f;
    public int monsterSpawnCount = 0;
    [Header("WayPoint"), Tooltip("몬스터 웨이포인트")]
    public Transform[] wayPointArray;
    [Header("Move"), Tooltip("몬스터 속도 제어")]
    public float moveSpeed = 10.0f;
    public float rotSpeed = 360.0f;

    //네브메쉬
    private NavMeshPath myPath;

    // 코루틴
    private Coroutine corMove = null;
    private Coroutine corRotate = null;
    private Coroutine corByPathMove = null;
    private Coroutine spawningCoroutine;

    private void Awake(){
        selectedMonster = null;
        allMonsterList = new List<Monster>();
    }

    protected void StopMoveCoroutine(){
        if (corMove != null) {
            StopCoroutine(corMove);
            corMove = null;
        }
        if (corRotate != null) {
            StopCoroutine(corRotate);
            corRotate = null;
        }
        if (corByPathMove != null) {
            StopCoroutine(corByPathMove);
            corByPathMove = null;
        }
    }

    // 무브
    public void Move(){
        if(myPath == null) myPath = new NavMeshPath();
        foreach(Monster monster in allMonsterList){
            if(monster.transform.position == wayPointArray[monster.curWayPointIdx].position){
                monster.curWayPointIdx++;
                if(monster.curWayPointIdx > 3) monster.curWayPointIdx = 0;
            }
            if(NavMesh.CalculatePath(monster.transform.position, wayPointArray[monster.curWayPointIdx].position, NavMesh.AllAreas, myPath)){
                switch(myPath.status){
                    case NavMeshPathStatus.PathComplete:
                    case NavMeshPathStatus.PathPartial:
                        StopMoveCoroutine();
                        corByPathMove = StartCoroutine(MovingByPath(myPath.corners, monster));
                        break;
                    case NavMeshPathStatus.PathInvalid:
                        break;
                }
            }
        }
    }

    IEnumerator MovingByPath(Vector3[] path, Monster monster){
        int curIdx = 1;
        while(curIdx < path.Length){
            yield return corMove = StartCoroutine(MovingToPos(path[curIdx++], monster));
        }
    }

    IEnumerator MovingToPos(Vector3 pos, Monster monster){
        Vector3 moveDir = pos - monster.transform.position;
        float moveDist = moveDir.magnitude;
        moveDir.Normalize();

        corRotate = StartCoroutine(RotatingToPos(moveDir, monster));

        while(moveDist > 0.0f){
            float delta = moveSpeed * Time.deltaTime;
            if(moveDist < delta) delta = moveDist;
            monster.transform.Translate(moveDir * delta, Space.World);
            moveDist -= delta;
            yield return null;
        }
    }
    IEnumerator RotatingToPos(Vector3 dir, Monster monster){
        float rotAngle = Vector3.Angle(monster.transform.forward, dir);
        
        float rotDir = 1.0f;
        if(Vector3.Dot(monster.transform.right, dir) < 0.0f) rotDir = -1.0f;

        while(rotAngle > 0.0f){
            float delta = rotSpeed * Time.deltaTime;
            if(rotAngle < delta) delta = rotAngle;
            monster.transform.Rotate(Vector3.up * rotDir * delta);
            rotAngle -= delta;
            yield return null;
        }
    }

    //스폰
    public void SpawnMonster()
    {
        GameObject obj = Instantiate(monsterPrefabArray[0], monsterSpawn);
        //obj.transform.parent = monsterSpawn;
        Monster monster = obj.GetComponent<Monster>();
        monster.curWayPointIdx = 1;
        allMonsterList.Add(monster);
        monsterSpawnCount++;
    }
}
