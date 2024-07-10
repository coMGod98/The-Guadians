using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public struct UnitStat
{
    // public string Type;
    // public char Rank;
    public float AttackDelay;
    public float AttackRange;
    public float AttackPoint;
    public int AttackType;
    public int Gold;
}

public class Unit : MonoBehaviour
{  
    public GameObject unitMarker;
    public Animator unitAnim;
    public int seedID;
    public Vector3 destination;
    public UnitStat unitStat;
    public List<Monster> target;
}
