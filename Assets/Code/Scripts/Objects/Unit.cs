using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;


[System.Serializable]
public struct UnitData
{
    public float AttackDuration;
    public float AttackCoolTime;
    public float AttackRange;
    public float AttackPoint;
    public int AttackType;
    public int Gold;
}

public class Unit : MonoBehaviour
{  

    public GameObject unitMarker;
    public UnitData unitData;
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
        unitData.AttackDuration = 1.0f;
        unitData.AttackCoolTime = 2.0f;
    }

    public bool IsAttacking => unitData.AttackDuration > attackElapsedTime;
    public bool IsAttackable => !IsAttacking && unitData.AttackCoolTime <= attackElapsedTime;
}
