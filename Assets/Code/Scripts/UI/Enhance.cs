using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.EventSystems;

public class Enhance : MonoBehaviour,IPointerClickHandler
{
    private string filePath;
   

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.dataPath + "/Code/Scripts/Object/UnitDB.xml";
    }
   public void OnPointerClick(PointerEventData eventData)
    {
        UpgaradePoint();
    }

    public void UpgaradePoint(params string[] unitNames)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);

        foreach (string unitName in unitNames)
        {
            XmlNode rowNode = xmlDoc.SelectSingleNode("/rows/row[unitName='" + unitName + "']");
            if (rowNode != null)
            {
                XmlNode attackPointNode = rowNode.SelectSingleNode("attackPoint");
                int currentAttackPoint = int.Parse(attackPointNode.InnerText);
                int newAttackPoint = currentAttackPoint + 5; 
                attackPointNode.InnerText = newAttackPoint.ToString();
            }
            else
            {
                Debug.LogWarning("Unit with name '" + unitName + "' not found. Cannot upgrade AttackPoint.");
            }
            xmlDoc.Save(filePath);
        }
    }

    public int LoadAttackPoint(string unitName)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);

        XmlNode aPnode = xmlDoc.SelectSingleNode("/rows/row[unitName='" + unitName + "']");
        if (aPnode != null)
        {
            XmlNode attackPointNode = aPnode.SelectSingleNode("attackPoint");
            int attackPoint = int.Parse(attackPointNode.InnerText);
            return attackPoint;
        }
        else
        {
            Debug.LogWarning("Unit with name '" + unitName + "' not found.");
            return 0;
        }

    }
    public void Update()
    {
        
    }





}
