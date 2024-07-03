using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Movement
{
    public enum State{
        Create, Normal, Battle, Death
    }
    public static List<Monster> allMonsterList = new List<Monster>();
    Transform[] wayPoint;
    int currentWaypointIndex = 1;


    public void SetWaypoint(Transform[] wayPointArray)
    {
        wayPoint = wayPointArray;
    }

    void Update(){
         MoveToPos(wayPoint[currentWaypointIndex].position);
         if(transform.position == wayPoint[currentWaypointIndex].position){
            currentWaypointIndex++;
            if(currentWaypointIndex > 3) currentWaypointIndex = 0;
         }
    }

}
