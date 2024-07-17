using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject[] bulletPrefabsArray;
    public List<Bullet> allBulletList = new List<Bullet>();

    private float _rotSpeed = 360.0f;

    public void Move()
    {
        foreach(Bullet bullet in allBulletList)
        {
            switch (bullet.bulletData.shootingType)
            {
                case BulletShootingType.Follow:
                {
                    Vector3 dir = bullet.targetMonster.transform.position - bullet.transform.position;
                    float dist = dir.magnitude;
                    dir.Normalize();

                    float rotAngle = Vector3.Angle(bullet.transform.forward, dir);
                    float rotDir = Vector3.Dot(bullet.transform.right, dir) < 0.0f ? -1.0f : 1.0f;

                    float rotateAmount = _rotSpeed * Time.deltaTime;
                    if (rotAngle < rotateAmount) rotateAmount = rotAngle;
                    bullet.transform.Rotate(Vector3.up * rotDir * rotateAmount);

                    float moveAmount = bullet.bulletData.speed * Time.deltaTime;
                    if (dist < moveAmount) moveAmount = dist;
                    bullet.transform.Translate(dir * moveAmount, Space.World);

                    break;
                }
                case BulletShootingType.Straight:
                {
                    bullet.transform.localPosition += transform.forward;
                    break;
                }
            }
        }
    }

    public void SpawnBullet(Unit unit)
    {
        Transform socket = unit.GetComponentInChildren<Socket>().transform;
        GameObject obj = Instantiate(bulletPrefabsArray[unit.unitData.bulletKey - 1], socket);
        Bullet bullet = obj.GetComponent<Bullet>();
        bullet.bulletKey = unit.unitData.bulletKey;
        bullet.targetMonster = unit.targetMonster;

        bullet.bulletData = GameWorld.Instance.BalanceManager.bulletDic[bullet.bulletKey];

        allBulletList.Add(bullet);
    }
}
