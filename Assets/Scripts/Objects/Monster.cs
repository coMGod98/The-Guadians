using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Sprite portrait;
    public string monsterKey;

    public Animator monsterAnim;
    public int curWayPointIdx;
    public MonsterData monsterData;
    public float curHP;

    public void Init()
    {
        monsterAnim = GetComponentInChildren<Animator>();   
        curWayPointIdx = 1;
        curHP = monsterData.HP;
    }
    
    public void InflictDamage(float dmg)
    {
        curHP -= dmg;
    }

    public bool IsDead => curHP <= 0;
}
