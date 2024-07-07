using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    [Header("UnitList"), Tooltip("유닛 리스트")]
    [SerializeField] public static List<Unit> allUnitList = new List<Unit>();
    [SerializeField] public static List<Unit> selectUnitList = new List<Unit>();

    [Header("Prefab"), Tooltip("유닛 프리팹")]
    public GameObject[] unitPrefabArray;
    [Header("Spawn"), Tooltip("유닛 스폰지")]
    public Transform unitSpawn;
    [Header("Move"), Tooltip("유닛 속도제어")]
    public float moveSpeed = 2.0f;
    public float rotSpeed = 360.0f;

    public List<float> distFromOtherUnit;


    // 네브메쉬패스
    private NavMeshPath myPath;

    // 코루틴
    Coroutine corMove = null;
    Coroutine corRotate = null;
    Coroutine corByPathMove = null;

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
    
    //무브
    public void on()
    {
		// for ( int i = 0; i < units.Count; ++ i ){
		// 	//units[i].Move(pos);
		// }
    }

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

    // 스폰
    public void SpawnUnit()
    {
        int randIdx = Random.Range(0, unitPrefabArray.Length);
        Vector3 randomSpawn = RandomSpawn();
        GameObject obj = Instantiate(unitPrefabArray[randIdx], randomSpawn, Quaternion.identity);
        obj.transform.parent = unitSpawn;
        Unit unit = obj.GetComponent<Unit>();

        allUnitList.Add(unit);
    }

    Vector3 RandomSpawn()
    {
        float radius = Random.Range(0.0f, 1.0f);
        float angle = Random.Range(0.0f, 360.0f);
        float x = radius * Mathf.Sin(angle);
        float z = radius * Mathf.Cos(angle);
        Vector3 randomVector = new Vector3(x, 0.55f, z);
        Vector3 randomPosition = unitSpawn.transform.position + randomVector;
        return randomPosition;
    }


    private void Update() {
        if (distFromOtherUnit != null) distFromOtherUnit = new List<float>();
        distFromOtherUnit.Clear();

        foreach(Unit unit in allUnitList){
            float dist = Vector3.Distance(unit.transform.position, transform.position);
            distFromOtherUnit.Add(dist);
        }
    }
}
