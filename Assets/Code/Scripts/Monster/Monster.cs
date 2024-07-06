using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : Movement
{
    public static List<Monster> allMonsterList = new List<Monster>();
    Transform[] wayPoint;
    int currentWaypointIndex = 1;

    public void SelectMonster(){
        gameObject.GetComponent<Outline>().enabled = true;
    }

    public void DeselectMonster(){
        gameObject.GetComponent<Outline>().enabled = false;
    }

    public void SetWaypoint(Transform[] wayPointArray)
    {
        wayPoint = wayPointArray;
    }

    void Update(){
        MoveToPos(wayPoint[currentWaypointIndex].position);
        if (transform.position == wayPoint[currentWaypointIndex].position) {
            currentWaypointIndex++;
            if (currentWaypointIndex > 3) currentWaypointIndex = 0;
        }
    }

}
