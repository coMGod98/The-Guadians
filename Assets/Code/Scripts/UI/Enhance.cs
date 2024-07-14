using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class Enhance : MonoBehaviour
{
    private string filePath;
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.dataPath + "/Code/Scripts/Object/UnitDB.xml";
    }

    public int LoadAttackPoint()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);
        XmlNode aPnode = xmlDoc.SelectSingleNode("/UnitDB/attackPoint");
        int attackPower = int.Parse(aPnode.InnerText);

        return attackPower;
        
    }
    public void SaveStats(int attackPoint)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);
        XmlNode aPnode = xmlDoc.SelectSingleNode("/UnitStats/attackPoint");
        aPnode.InnerText = aPnode.ToString();
        xmlDoc.Save(filePath);
    }

    public void Increase(int AttackinCrease)
    {
        int curPoint = LoadAttackPoint();
        int newPoint = curPoint + AttackinCrease;
        SaveStats(newPoint);
    }
     
    
}
