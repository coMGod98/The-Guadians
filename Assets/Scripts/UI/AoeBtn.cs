using UnityEngine;
using EasyUI.Toast;

public class AoeBtn : MonoBehaviour
{
    public int cost;

    private void Awake()
    {
        cost = 20;
    }

    public void Aoe(int aoeIndex)
    {
        if(GameWorld.Instance.UIManager.isButtonLocked)
        {
            Toast.Show("Skill Not Used Yet", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
            return;
        }
        else
        {
            if (GameWorld.Instance.playerGolds < cost)
            {
               GameWorld.Instance.UIManager.Alert(cost, GameWorld.Instance.playerGolds);
            }
            else
            {
                GameWorld.Instance.TakeGold(cost);
                GameWorld.Instance.UIManager.isButtonLocked = true;
                GameWorld.Instance.AoeManager.ButtonClick(aoeIndex);
            }
        }
    }

    public void RButton1() => Aoe(0);
    public void RButton2() => Aoe(1);
    public void RButton3() => Aoe(2);
    public void RButton4() => Aoe(3);
}
