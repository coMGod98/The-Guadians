using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Normal, Hold, Move, Combat
}

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

    //유닛 랜덤
    private Dictionary<string, double> dicRank;
    double SumOfWeights;


    // 네브메쉬패스
    private NavMeshPath myPath;

    private void Awake(){
        allUnitList = new List<Unit>();
        selectedUnitList = new List<Unit>();

        Dictionary<string, double> dicRank = new Dictionary<string, double>()
        {
            {"N", 60.0f},
            {"R", 30.0f},
            {"E", 12.4f},
            {"U", 2.4f},
            {"L", 0.01f}
        };

        foreach(float value in dicRank.Values){
            SumOfWeights += value;
        }
    }

    public void UnitAI(){
        foreach(Unit unit in allUnitList)
        {
            foreach(Monster monster in GameWorld.Instance.MonsterManager.allMonsterList)
            {
                if(Vector3.Distance(unit.transform.position, monster.transform.position) < unit.unitStat.AttackRange)
                {
                    if(!unit.rangeMonster.Contains(monster)) unit.rangeMonster.Add(monster);
                }
                else
                {
                    if(unit.rangeMonster.Contains(monster)) unit.rangeMonster.Remove(monster);
                }

                if (unit.rangeMonster.Count > 0 && unit.unitState != State.Combat) 
                {
                    if (unit.unitState == State.Normal) unit.unitState = State.Combat;
                    unit.targetMonster = unit.rangeMonster[0];
                }

                switch (unit.unitState)
                {
                    case State.Combat:
                    {
                        if (unit.targetMonster != null && unit.unitAnim.GetBool("IsAttacking") == false && unit.unitAnim.GetBool("IsMoving") == false) 
                        {
                            unit.destination = unit.targetMonster.transform.position;
                            OnAttack(unit);
                        }

                    }
                        break;
                    case State.Hold:
                    {
                        if(unit.targetMonster != null)
                        OnAttack(unit);
                    }
                        break;
                }
            }
        }
    }

    public void OnHold()
    {
        foreach (Unit unit in selectedUnitList)
        {
            unit.unitState = State.Hold;
        }
    }

    public void OnAttack(Unit unit){
        if(Vector3.Distance(unit.transform.position, unit.targetMonster.transform.position) < unit.unitStat.AttackRange)
        {
            if(unit.unitAnim.GetBool("IsAttacking") == false && unit.unitAnim.GetBool("IsMoving") == false) 
            {
                Vector3 dir = unit.targetMonster.transform.position - unit.transform.position;
                dir.Normalize();
                Rotate(dir, unit);
                
                unit.unitAnim.SetTrigger("OnAttack");
            }
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

                        Rotate(moveDir, unit);
            
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

    public void InputDestination(Vector3 pos)
    {
        List<Vector3> destinationList = GetDestinationListAround(pos, new float[] { 2.0f, 4.0f, 6.0f}, new int[] { 5, 10, 20 });
        
        int destinationListIdx = 0;

        foreach(Unit unit in selectedUnitList){
            unit.destination = destinationList[destinationListIdx];
            destinationListIdx = (destinationListIdx + 1) % destinationList.Count;
            
            if(unit.targetMonster != null) {
                unit.targetMonster = null;
                unit.unitState = State.Normal;
            }
        }
    }

    private List<Vector3> GetDestinationListAround(Vector3 pos, float[] ringRadiusArray, int[] ringPositionCount){
        List<Vector3> destinatnionList = new List<Vector3>();
        destinatnionList.Add(pos);
        for(int i = 0; i < ringRadiusArray.Length; i++){
            destinatnionList.AddRange(GetDestinationListAround(pos, ringRadiusArray[i], ringPositionCount[i]));
        }
        return destinatnionList;
    }

    private List<Vector3> GetDestinationListAround(Vector3 pos, float radius, int positionCount){
        List<Vector3> destinationList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360.0f / positionCount);
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            Vector3 dir = new Vector3(x, 0.0f, z);
            Vector3 destination = pos + dir * radius;
            destinationList.Add(destination);
        }
        return destinationList;
    }

    void Rotate(Vector3 dir, Unit unit){
        float rotAngle = Vector3.Angle(unit.transform.forward, dir);
        float rotDir = Vector3.Dot(unit.transform.right, dir) < 0.0f ? -1.0f : 1.0f;

        float rotateAmount = rotSpeed * Time.deltaTime;
        if(rotAngle < rotateAmount) rotateAmount = rotAngle;
        unit.transform.Rotate(Vector3.up * rotDir * rotateAmount);
    }


    // 스폰
    public void SpawnUnit()
    {
        int randIdx = Random.Range(0, unitPrefabArray.Length);
        Vector3 randomSpawn = RandomSpawn();
        GameObject obj = Instantiate(unitPrefabArray[randIdx], randomSpawn, Quaternion.identity);
        obj.transform.parent = unitSpawn;
        Unit unit = obj.GetComponent<Unit>();
        string rank = GetRandomPick();
        int index = unit.name.IndexOf("(Clone)");
        unit.name = unit.name.Substring(0, index) + rank;
        UnitDB.instance.LoadUnitStatFromXML(unit.name, unit);

        unit.Init();
        switch(rank)
        {
            case "N":
            unit.outline.enabled = false;
            break;
            case "R":
            unit.outline.outlineColor = Color.green;
            break;
            case "E":
            unit.outline.outlineColor = Color.cyan;
            break;
            case "U":
            unit.outline.outlineColor = Color.yellow;
            break;
            case "L":
            unit.outline.outlineColor = Color.magenta;
            break;
        }

        //unit.unitStat.AttackRange = 5.0f;


        allUnitList.Add(unit);
    }

    Vector3 RandomSpawn()
    {
        float radius = Random.Range(0.0f, 2.0f);
        float angle = Random.Range(0.0f, 360.0f);
        float x = radius * Mathf.Sin(angle);
        float z = radius * Mathf.Cos(angle);
        Vector3 randomVector = new Vector3(x, 0.55f, z);
        Vector3 randomPosition = unitSpawn.transform.position + randomVector;
        return randomPosition;
    }

    public string GetRandomPick(){
        Dictionary<string, double> dicRank = new Dictionary<string, double>()
        {
            {"N", 60.0f},
            {"R", 30.0f},
            {"E", 12.4f},
            {"U", 2.4f},
            {"L", 0.01f}
        };
        
        double randomValue = Random.Range(0.0f, 1.0f);
        randomValue *= SumOfWeights;

        if(randomValue < 0.0) randomValue = 0.0f;
        if(randomValue > SumOfWeights) randomValue = SumOfWeights - 0.00000001;

        double current = 0.0f;
        foreach(KeyValuePair<string, double> item in dicRank)
        {
            current += item.Value;

            if(randomValue < current)
            {
                return item.Key;
            }
        }
        
        throw new System.Exception($"Unreachable - [Random Value : {randomValue}, Current Value : {current}]");
    }


}