using UnityEngine;
using EasyUI.Toast;

public class AoeBtn : MonoBehaviour
{
    public int cost;

    private void Awake()
    {
        cost = 20;
    }

    private void Start()
    {
        GameWorld.Instance.AoeManager.MeteoPlaced += OnMeteoPlaced;
        GameWorld.Instance.AoeManager.SnowPlaced += OnSnowPlaced;
        GameWorld.Instance.AoeManager.BomBPlaced += OnBomBPlaced;
    }

    private void OnDestroy()
    {
        GameWorld.Instance.AoeManager.MeteoPlaced -= OnMeteoPlaced;
        GameWorld.Instance.AoeManager.SnowPlaced -= OnSnowPlaced;
        GameWorld.Instance.AoeManager.BomBPlaced -= OnBomBPlaced;
    }

    private void OnMeteoPlaced()
    {
        GameWorld.Instance.UIManager.isMeteoLocked = false;
    }

    private void OnSnowPlaced()
    {
        GameWorld.Instance.UIManager.isSnowLocked = false;
    }

    private void OnBomBPlaced()
    {
        GameWorld.Instance.UIManager.isBomBLocked = false;
    }

    public void Aoe(int aoeIndex)
    {
        if (GameWorld.Instance.AoeManager.IsAoeSelected())
        {
            Toast.Show("이미 다른 스킬을 선택했습니다.", 2f, ToastColor.Black, ToastPosition.MiddleCenter); // alredy choose skills
            return;
        }

        bool isButtonLocked = false;

        switch (aoeIndex)
        {
            case 0: isButtonLocked = GameWorld.Instance.UIManager.isMeteoLocked; break;
            case 1: isButtonLocked = GameWorld.Instance.UIManager.isSnowLocked; break;
            case 2: isButtonLocked = GameWorld.Instance.UIManager.isBomBLocked; break;
        }

        if (isButtonLocked)
        {
            Toast.Show("아직 스킬 사용이 되지 않았습니다", 2f, ToastColor.Black, ToastPosition.MiddleCenter); // skill not used yet
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

                switch (aoeIndex)
                {
                    case 0: GameWorld.Instance.UIManager.isMeteoLocked = true; break;
                    case 1: GameWorld.Instance.UIManager.isSnowLocked = true; break;
                    case 2: GameWorld.Instance.UIManager.isBomBLocked = true; break;
                }

                GameWorld.Instance.AoeManager.ButtonClick(aoeIndex);
            }
        }
    }

    public void RButton1() => Aoe(0); 
    public void RButton2() => Aoe(1); 
    public void RButton3() => Aoe(2); 
}
