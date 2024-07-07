using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] public static List<Monster> allMonsterList = new List<Monster>();
    [SerializeField] public static List<Unit> selectUnitList = new List<Unit>();

    Transform[] wayPoint;
    int currentWaypointIndex = 1;

    public void SetWaypoint(Transform[] wayPointArray)
    {
        wayPoint = wayPointArray;
    }

    void Update(){
        //MoveToPos(wayPoint[currentWaypointIndex].position);
        if (transform.position == wayPoint[currentWaypointIndex].position) {
            currentWaypointIndex++;
            if (currentWaypointIndex > 3) currentWaypointIndex = 0;
        }
    }

}
