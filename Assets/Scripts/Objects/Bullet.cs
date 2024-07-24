using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public int bulletKey;
    public int hitArrayIdx;
    public Unit bulletOwner;
    public Monster targetMonster;
    public List<Monster> hitMonsterList;
    

    public BulletData bulletData;
}
