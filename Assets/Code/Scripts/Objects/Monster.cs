using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct MonsterStat
{
    public float HP;
    public int Gold;
    public float Speed;
}

public class Monster : MonoBehaviour
{
    public Animator monsterAnim;
    public int curWayPointIdx;
    public MonsterStat monsterStat;
    public float curHP;

    public void Init()
    {
        monsterAnim = GetComponentInChildren<Animator>();   
        curWayPointIdx = 1;
        curHP = monsterStat.HP;
    }
    
    public void InflictDamage(float dmg)
    {
        curHP -= dmg;
    }

    public bool IsDead => curHP <= 0;
}
