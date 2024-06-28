using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitController : MonoBehaviour
{

    public List<Unit> selectUnitList = new List<Unit>();
    public List<Unit> allUnitList = new List<Unit>();

    void SelectUnit(Unit newUnit){
        newUnit.SelectUnit();
        selectUnitList.Add(newUnit);
    }
    void DeselectUnit(Unit newUnit){
        newUnit.DeselectUnit();
        selectUnitList.Remove(newUnit);

    }
    public void DeselectAll(){
        if (selectUnitList.Count > 0)
        {
            for (int i = 0; i < selectUnitList.Count; i++)
            {
                selectUnitList[i].DeselectUnit();
            }
            selectUnitList.Clear();
        }
    }
    public void OneSelectUnit(Unit newUnit){
        DeselectAll();
        SelectUnit(newUnit);
        Debug.Log(selectUnitList.Count);
    }
    public void MulSelectAct(Unit newUnit){
        if(selectUnitList.Contains(newUnit)) DeselectUnit(newUnit);
        else SelectUnit(newUnit);
        Debug.Log(selectUnitList.Count);
    }
}
