using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct MonsterStat
{
    public int Round;
    public float HP;
    public int Gold;
    public float Speed;
}

public class Monster : MonoBehaviour
{
    public Animator monsterAnim;
    public int curWayPointIdx;
    public MonsterStat monsterStat;
}
