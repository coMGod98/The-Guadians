using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;


[System.Serializable]
public struct UnitStat
{
    public float AttackDelay;
    public float AttackRange;
    public float AttackPoint;
    public int AttackType;
    public int Gold;
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
    public float attackDuration = 1.0f;
    public float attackElapsedTime;
    public float attackCoolTime = 2.0f;
    public bool forceMove = false;


    public void Init(){

        unitState = State.Normal;
        unitAnim = GetComponentInChildren<Animator>();
        outline = GetComponent<Outline>();
        destination = transform.position;
        targetMonster = null;
    }

    public bool IsAttacking => attackDuration > attackElapsedTime;
    public bool IsAttackable => !IsAttacking && attackCoolTime <= attackElapsedTime;
}
