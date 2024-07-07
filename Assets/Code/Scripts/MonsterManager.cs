using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterManager : MonoBehaviour
{
    [Header("SelectedMonster"), Tooltip("몬스터 선택")]
    public Monster selectedMonster;
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
    public float moveSpeed = 2.0f;
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
    public void MoveToPos(Vector3 pos){
        if(myPath == null) myPath = new NavMeshPath();
        if(NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, myPath)){
            switch(myPath.status){
                case NavMeshPathStatus.PathComplete:
                case NavMeshPathStatus.PathPartial:
                    StopMoveCoroutine();
                    corByPathMove = StartCoroutine(MovingByPath(myPath.corners));
                    break;
                case NavMeshPathStatus.PathInvalid:
                    break;
            }
        }
    }

    IEnumerator MovingByPath(Vector3[] path){
        int curIdx = 1;
        while(curIdx < path.Length){
            yield return corMove = StartCoroutine(MovingToPos(path[curIdx++]));
        }
    }

    IEnumerator MovingToPos(Vector3 pos){
        Vector3 moveDir = pos - transform.position;
        float moveDist = moveDir.magnitude;
        moveDir.Normalize();

        corRotate = StartCoroutine(RotatingToPos(moveDir));

        while(moveDist > 0.0f){
            float delta = moveSpeed * Time.deltaTime;
            if(moveDist < delta) delta = moveDist;
            transform.Translate(moveDir * delta, Space.World);
            moveDist -= delta;
            yield return null;
        }
    }
    IEnumerator RotatingToPos(Vector3 dir){
        float rotAngle = Vector3.Angle(transform.forward, dir);
        
        float rotDir = 1.0f;
        if(Vector3.Dot(transform.right, dir) < 0.0f) rotDir = -1.0f;

        while(rotAngle > 0.0f){
            float delta = rotSpeed * Time.deltaTime;
            if(rotAngle < delta) delta = rotAngle;
            transform.Rotate(Vector3.up * rotDir * delta);
            rotAngle -= delta;
            yield return null;
        }
    }

    //스폰
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
