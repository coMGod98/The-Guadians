using System.Collections.Generic;
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

    // 네브메쉬패스
    private NavMeshPath myPath;

    private void Awake(){
        allUnitList = new List<Unit>();
        selectedUnitList = new List<Unit>();
    }
    
    //무브
    // NavMeshAgent
    // public void Move(){
    //     if(myPath == null) myPath = new NavMeshPath();
    //     foreach(Unit unit in allUnitList){
    //         unit.myNavagent.SetDestination(unit.destination);
    //     }
    // }

    public void UnitAI(){
        foreach(Unit unit in allUnitList)
        {
            foreach(Monster monster in GameWorld.Instance.MonsterManager.allMonsterList)
            {
                if(Vector3.Distance(unit.transform.position, monster.transform.position) < unit.unitStat.AttackRange)
                {
                    if (!unit.rangeMonster.Contains(monster)) unit.rangeMonster.Add(monster);
                }
                else
                {
                    if(unit.rangeMonster.Contains(monster)) unit.rangeMonster.Remove(monster);
                }

                if(unit.rangeMonster.Count > 0 || unit.targetMonster != null)
                {
                    if(unit.attackElapse > 3.0f && !unit.rangeMonster.Contains(unit.targetMonster) && unit.targetMonster != null)
                    {
                        unit.targetMonster = unit.rangeMonster[0];
                        unit.attackElapse = 0.0f;
                    }
                    else if (unit.targetMonster == null)
                    {
                        unit.targetMonster = unit.rangeMonster[0];
                    }
                    unit.attackElapse += Time.deltaTime;
                    unit.destination = unit.targetMonster.transform.position;
                }



            }
        }
    }

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
                        unit.unitAnim.SetBool("IsMoving", true);
                        Vector3 moveDir = myPath.corners[1] - unit.transform.position;
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
                        unit.unitAnim.SetBool("IsMoving", false);
                        unit.destination = unit.transform.position;
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

        int index = unit.name.IndexOf("(Clone)");
        string name = unit.name.Substring(0, index);
        UnitDB.instance.LoadUnitStatFromXML(name, unit);

        //unit.myNavagent = unit.GetComponent<NavMeshAgent>();
        unit.unitAnim = unit.GetComponentInChildren<Animator>();

        unit.seedID = _seedNum++;
        unit.rangeMonster = new List<Monster>();
        unit.targetMonster = null;

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