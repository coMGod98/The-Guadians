using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class Excel : MonoBehaviour
{
    public static Excel instance;

    public TextAsset unitDBFileXml;

   public struct UnitState
    {
        public string unitType;
        public char unitRank;
        public float attackDealy;
        public float attackRange;
        public float attackPoint;
        public int attackType;
        public int unitGold;
    }

    Dictionary<string, Dictionary<char, UnitState>> dicUnits = new Dictionary<string, Dictionary<char, UnitState>>();
    Dictionary<char, UnitState> dic = new Dictionary<char, UnitState>();
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        MakeUnitXML();
    }

    public void MakeUnitXML()
    {
        XmlDocument unitXMLDoc = new XmlDocument();
        unitXMLDoc.LoadXml(unitDBFileXml.text);

        XmlNodeList unitNodeList = unitXMLDoc.GetElementsByTagName("row");

        foreach (XmlNode unitNode in unitNodeList)
        {
            UnitState unitState = new UnitState();

            foreach (XmlNode childNode in unitNode.ChildNodes)
            {
                if (childNode.Name == "_unitType")
                {
                    unitState.unitType = childNode.InnerText;
                }

                if (childNode.Name == "_unitRank")
                {
                    unitState.unitRank = char.Parse(childNode.InnerText);
                }

                if (childNode.Name == "_attackDealy")
                {
                    unitState.attackDealy = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "_attackRange")
                {
                    unitState.attackRange = Int16.Parse(childNode.InnerText);
                }
                if (childNode.Name == "_attackPoint")
                {
                    unitState.attackPoint = Int16.Parse(childNode.InnerText);
                }
                if (childNode.Name == "_attackType")
                {
                    unitState.attackType = Int16.Parse(childNode.InnerText);
                }
                if (childNode.Name == "_unitGold")
                {
                    unitState.unitGold = Int16.Parse(childNode.InnerText);
                }
                print(childNode.Name + ": " + childNode.InnerText);
            }
            dic[unitState.unitRank] = unitState;
            dicUnits[unitState.unitType] = dic;
        }
    }

   public void LoadMonsterParamsFromXML(string unitName, Unit unit)
    {
        string type = unitName[..^1];
        char rank = unitName[^1];

        unit.unitStat.AttackDelay = dicUnits[type][rank].attackDealy;
        unit.unitStat.AttackRange = dicUnits[type][rank].attackRange;
        unit.unitStat.AttackPoint = dicUnits[type][rank].attackPoint;
        unit.unitStat.AttackType = dicUnits[type][rank].attackType;
        unit.unitStat.Gold =  dicUnits[type][rank].unitGold;
    }
}