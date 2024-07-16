using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceController : MonoBehaviour
{
    public Enhance enhance;
    
    // Start is called before the first frame update
    void Start()
    {
        enhance= GetComponent<Enhance>();

        Button upgradeButton = GetComponent<Button>();
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
        }
        else
        {
            Debug.LogWarning("Upgrade Button not found.");
        }
    }
    void OnUpgradeButtonClick()
    {
        
       enhance.UpgaradePoint("WarriorN", "WarriorR", "WarriorE", "WarriorU", "WarriorL");

    
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
