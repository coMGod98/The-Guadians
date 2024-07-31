using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.Toast;

public class SpawnUnitsBtn : MonoBehaviour
{
    public int cost;

    private void Awake()
    {
        cost = 5;
    }

    public void SpawnUnit()
    {
        if (GameWorld.Instance.playerGolds < cost)
        {
            GameWorld.Instance.UIManager.Alert(cost, GameWorld.Instance.playerGolds);
        }
        else
        {
            GameWorld.Instance.TakeGold(cost);
            GameWorld.Instance.UnitManager.SpawnUnit();
        }
    }
}
