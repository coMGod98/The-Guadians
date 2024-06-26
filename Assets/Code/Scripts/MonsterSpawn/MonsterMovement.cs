using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterMovement : MonoBehaviour
{
    public float speed = 2.0f;
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    public void SetWaypoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
        transform.position = waypoints[0].position;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                Destroy(gameObject);
            }
        }
    }
}
