using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Movement
{
    public enum State{
        Create, Normal, Battle, Death
    }
    public static List<Monster> allMonsterList = new List<Monster>();
    Transform[] wayPointList;
    int currentWaypointIndex = 1;


    public void SetWaypoint(Transform[] wayPoint)
    {
        wayPointList = wayPoint;
    }

    void Update(){
         MoveToPos(wayPointList[currentWaypointIndex].position);
         if(transform.position == wayPointList[currentWaypointIndex].position){
            currentWaypointIndex++;
            if(currentWaypointIndex > 3) currentWaypointIndex = 0;
         }
    }

}
