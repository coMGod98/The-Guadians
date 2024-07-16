using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{  
    public GameObject unitMarker;
    public UnitData unitData;
    public Animator unitAnim;
    public Outline outline;
    
    public int upgarde;

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
        unitData.attackDuration = 1.0f;
        unitData.attackCoolTime = 2.0f;
    }

    public bool IsAttacking => unitData.attackDuration > attackElapsedTime;
    public bool IsAttackable => !IsAttacking && unitData.attackCoolTime <= attackElapsedTime;
}
