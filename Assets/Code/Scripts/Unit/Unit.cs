using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Movement
{
    public static List<Unit> allUnitList = new List<Unit>();

    public GameObject unitMarker;

    public void SelectUnit(){
        unitMarker.SetActive(true);
    }
    public void DeselectUnit(){
        unitMarker.SetActive(false);
    }
    public void Move(Vector3 pos){
        MoveToPos(pos);
    }
}
