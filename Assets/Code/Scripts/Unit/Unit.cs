using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Movement
{
    public static List<Unit> allUnitList = new List<Unit>();
    public GameObject unitMarker;
    public List<float> distFromOtherUnit;

    public void SelectUnit(){
        unitMarker.SetActive(true);
    }
    public void DeselectUnit(){
        unitMarker.SetActive(false);
    }
    
    public void Move(Vector3 pos){
        MoveToPos(pos);
    }

    private void Update() {
        distFromOtherUnit = new List<float>();
        foreach(Unit unit in allUnitList){
            float dist = Vector3.Distance(unit.transform.position, transform.position);
            distFromOtherUnit.Add(dist);
            if(dist < 1.0f && dist > 0.1f){
                // 유닛 간 거리가 가까우면 밀어내기
            }
        }
    }

}
