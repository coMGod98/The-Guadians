using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting.APIUpdating;

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


    // 네브메쉬패스
    private NavMeshPath myPath;

    // 코루틴
    Coroutine corMove = null;
    Coroutine corRotate = null;
    Coroutine corByPathMove = null;

    private void Awake(){
        allUnitList = new List<Unit>();
        selectedUnitList = new List<Unit>();
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
    // NavMeshAgent
    // public void Move(){
    //     if(myPath == null) myPath = new NavMeshPath();
    //     foreach(Unit unit in allUnitList){
    //         unit.myNavagent.SetDestination(unit.destination);
    //     }
    // }
    public void SetDesinationdUnits(Vector3 pos)
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
                    if(myPath.corners.Length > 1){
                        unit.myAnim.SetBool("IsMoving", true);
                        Vector3 corner = myPath.corners[1];
                        Vector3 moveDir = corner - unit.transform.position;
                        float moveDist = moveDir.magnitude;
                        moveDir.Normalize();
                        float rotAngle = Vector3.Angle(unit.transform.forward, moveDir);
                        
                        float rotDir = 1.0f;
                        if(Vector3.Dot(unit.transform.right, moveDir) < 0.0f) rotDir = -1.0f;

                        float rotateAmount = rotSpeed * Time.deltaTime;
                        if(rotAngle < rotateAmount) rotateAmount = rotAngle;
                        unit.transform.Rotate(Vector3.up * rotDir * rotateAmount);
            
                        float moveAmount = moveSpeed * Time.deltaTime;
                        if(moveDist < moveAmount) moveAmount = moveDist;
                        unit.transform.Translate(moveDir * moveAmount, Space.World);
                    }
                    else{
                        unit.myAnim.SetBool("IsMoving", false);
                    }
                        break;
                    case NavMeshPathStatus.PathInvalid:
                        break;
                }
            }
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
        //unit.myNavagent = unit.GetComponent<NavMeshAgent>();
        unit.myAnim = unit.GetComponentInChildren<Animator>();
        unit.seedID = _seedNum++;

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