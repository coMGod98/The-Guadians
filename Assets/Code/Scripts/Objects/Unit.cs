using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;


[System.Serializable]
public struct UnitStat
{
    public float AttackDuration;
    public float AttackCoolTime;
    public float AttackRange;
    public float AttackPoint;
    public int AttackType;
    public int Gold;

    public float AttackDelay;
}

public class Unit : MonoBehaviour
{  

    public GameObject unitMarker;
    public UnitStat unitStat;
    public Animator unitAnim;
    public Outline outline;
    
    public State unitState;
    public Vector3 destination;
    public Monster targetMonster;
    public float attackElapsedTime;
    public bool forceMove = false;


    public void Init(){

        unitState = State.Normal;
        unitAnim = GetComponentInChildren<Animator>();
        outline = GetComponent<Outline>();
        destination = transform.position;
        targetMonster = null;
        unitStat.AttackDuration = 1.0f;
        unitStat.AttackCoolTime = 2.0f;
    }

    public bool IsAttacking => unitStat.AttackDuration > attackElapsedTime;
    public bool IsAttackable => !IsAttacking && unitStat.AttackCoolTime <= attackElapsedTime;
}
