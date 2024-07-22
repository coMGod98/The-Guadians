using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.Toast;

public class ToastMSG : MonoBehaviour
{
    public void ShowMSG()
    {
        Toast.Show("골드가 부족합니다", 2f, ToastColor.Magenta, ToastPosition.MiddleCenter);
    }
}
