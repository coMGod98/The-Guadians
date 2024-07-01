using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : Movement
{
    public enum State{
        Create, Normal, Battle, Death
    }
    public static List<Monster> allMonsterList = new List<Monster>();
    Transform[] wayPoint;
    int currentWaypointIndex = 1;


    public void SetWaypoint(Transform[] wayPoint)
    {
        this.wayPoint = wayPoint;
    }

    void Update(){
        MoveToPos(wayPoint[currentWaypointIndex].position);
        if(Vector3.Distance(transform.position, wayPoint[currentWaypointIndex].position) < 1.0f){
            currentWaypointIndex++;
            if(currentWaypointIndex > 3) currentWaypointIndex = 0;
        }
    }
    
}
