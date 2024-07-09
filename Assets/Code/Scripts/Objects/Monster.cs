using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct MonsterStat
{
    public int Type;
    public float maxHealPoint;
    public float curHealPoint;
}

public class Monster : MonoBehaviour
{
    public Animator monsterAnim;
    public int curWayPointIdx;
    public MonsterStat monsterStat;
}
