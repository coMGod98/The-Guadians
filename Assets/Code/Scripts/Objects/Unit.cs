using System;
using UnityEngine;

[Serializable]
public struct UnitStat
{
    public int Job;
    public int Rank;
    public float AttackDelay;
    public float AttackRange;
    public float AttackPoint;
}

public class Unit : MonoBehaviour
{  
    public GameObject unitMarker;
    public Animator unitAnim;
    public int seedID;
    public Vector3 destination;
    public UnitStat unitStat;
}
