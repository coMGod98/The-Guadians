using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitType unitKey;

    public GameObject unitMarker;
    public UnitData unitData;
    public Animator unitAnim;
    public Outline outline;

    public int upgrade;

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
    }

    public bool IsAttacking => unitData.attackDuration > attackElapsedTime;
    public bool IsAttackable => !IsAttacking && unitData.attackCoolTime <= attackElapsedTime;
    public float unitDamage => unitData.attackDamage[upgrade];
}
