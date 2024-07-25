using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public UnitType unitKey;
    public UnitData unitData;

    public GameObject unitMarker;
    public Animator unitAnim;
    public Outline outline;

    public int upgrade;

    public State unitState;
    public Vector3 destination;
    public Monster targetMonster;
    public float prevElapsedTime;
    public float attackElapsedTime;
    public bool forceMove = false;
    public bool forceHold = false;

    public void Init()
    {

        unitState = State.Normal;
        unitAnim = GetComponentInChildren<Animator>();
        outline = GetComponent<Outline>();
        destination = transform.position;
        targetMonster = null;
    }

    public bool IsAttacking => unitData.attackDuration > attackElapsedTime;
    public bool IsCoolTimeDone => !IsAttacking && unitData.attackCoolTime <= attackElapsedTime;
    public bool IsAttackable => IsCoolTimeDone && Vector3.Distance(transform.position, targetMonster.transform.position) <= unitData.attackRange;
    public float unitDamage => unitData.attackDamage[upgrade];
}
