using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    [Header("UnitList"), Tooltip("유닛 리스트")]
    public List<Unit> allUnitList;
    public List<Unit> selectedUnitList;

    [Header("Prefab"), Tooltip("유닛 프리팹")]
    public GameObject[] unitPrefabArray;
    [Header("Spawn"), Tooltip("유닛 스폰지")]
    public Transform unitSpawn;
    [Header("Move"), Tooltip("유닛 속도제어")]
    public float moveSpeed = 5.0f;
    public float rotSpeed = 360.0f;

    private int _seedNum = 0;
    [SerializeField] public List<List<float>> distFromOtherUnit;


    // 네브메쉬패스
    private NavMeshPath myPath;

    // 코루틴
    Coroutine corMove = null;
    Coroutine corRotate = null;
    Coroutine corByPathMove = null;

    private void Awake(){
        allUnitList = new List<Unit>();
        selectedUnitList = new List<Unit>();
        distFromOtherUnit = new List<List<float>>();
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
    
    //무브
    public void CalculateDistBWUnits(){
        if(allUnitList.Count > 1){
            distFromOtherUnit.Clear();
            for (int i = 0; i < allUnitList.Count; i++)
            {
                distFromOtherUnit.Add(new List<float>());
                for (int j = 0; j < allUnitList.Count; j++)
                {
                    float dist = Vector3.Distance(allUnitList[i].transform.position, allUnitList[j].transform.position);
                    distFromOtherUnit[i].Add(dist);
                }
            }
        }
    }

    public void MoveSelectedUnit(Vector3 pos)
    {
        foreach(Unit unit in selectedUnitList){
            unit.destination = pos;
        }
    }
    
    public void Move(){
        if(myPath == null) myPath = new NavMeshPath();
        foreach(Unit unit in allUnitList){
            if(NavMesh.CalculatePath(unit.transform.position, unit.destination, NavMesh.AllAreas, myPath)){
                switch(myPath.status){
                    case NavMeshPathStatus.PathComplete:
                    case NavMeshPathStatus.PathPartial:
                        StopMoveCoroutine();
                        corByPathMove = StartCoroutine(MovingByPath(myPath.corners, unit));
                        break;
                    case NavMeshPathStatus.PathInvalid:
                        break;
                }
            }
        }
    }

    IEnumerator MovingByPath(Vector3[] path, Unit unit){
        int curIdx = 1;
        while(curIdx < path.Length){
            yield return corMove = StartCoroutine(MovingToPos(path[curIdx++], unit));
        }
    }

    IEnumerator MovingToPos(Vector3 pos, Unit unit){
        Vector3 moveDir = pos - unit.transform.position;
        float moveDist = moveDir.magnitude;
        moveDir.Normalize();

        corRotate = StartCoroutine(RotatingToPos(moveDir, unit));

        while(moveDist > 0.0f){
            float delta = moveSpeed * Time.deltaTime;
            if(moveDist < delta) delta = moveDist;
            unit.transform.Translate(moveDir * delta, Space.World);
            moveDist -= delta;

            yield return null;
        }
    }
    IEnumerator RotatingToPos(Vector3 dir, Unit unit){
        float rotAngle = Vector3.Angle(unit.transform.forward, dir);
        
        float rotDir = 1.0f;
        if(Vector3.Dot(unit.transform.right, dir) < 0.0f) rotDir = -1.0f;

        while(rotAngle > 0.0f){
            float delta = rotSpeed * Time.deltaTime;
            if(rotAngle < delta) delta = rotAngle;
            unit.transform.Rotate(Vector3.up * rotDir * delta);
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
        unit.seedID = _seedNum++;
        unit.destination = unit.transform.position;
        unit.target = null;

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
}