using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class UnitDB : MonoBehaviour
{
    public static UnitDB instance;

    public TextAsset unitDBFileXml;

    public struct UnitStat
    {
        public string unitType;
        public char unitRank;
        public float attackDelay;
        public float attackRange;
        public float attackPoint;
        public int attackType;
        public int unitGold;
    }

    Dictionary<string, Dictionary<char, UnitStat>> dicUnits = new Dictionary<string, Dictionary<char, UnitStat>>();
    Dictionary<char, UnitStat> dic = new Dictionary<char, UnitStat>();
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
            UnitStat unitStat = new UnitStat();

            foreach (XmlNode childNode in unitNode.ChildNodes)
            {
                if (childNode.Name == "unitType")
                {
                    unitStat.unitType = childNode.InnerText;
                }

                if (childNode.Name == "unitRank")
                {
                    unitStat.unitRank = char.Parse(childNode.InnerText);
                }

                if (childNode.Name == "attackDelay")
                {
                    unitStat.attackDelay = float.Parse(childNode.InnerText);
                }

                if (childNode.Name == "attackRange")
                {
                    unitStat.attackRange = float.Parse(childNode.InnerText);
                }
                if (childNode.Name == "attackPoint")
                {
                    unitStat.attackPoint = float.Parse(childNode.InnerText);
                }
                if (childNode.Name == "attackType")
                {
                    unitStat.attackType = int.Parse(childNode.InnerText);
                }
                if (childNode.Name == "unitGold")
                {
                    unitStat.unitGold = int.Parse(childNode.InnerText);
                }
            }
            dic[unitStat.unitRank] = unitStat;
            dicUnits[unitStat.unitType] = dic;
        }
    }

    public void LoadUnitStatFromXML(string unitName, Unit unit)
    {
        string type = unitName[..^1];
        char rank = unitName[^1];

        unit.unitStat.Type = dicUnits[type][rank].unitType;
        unit.unitStat.Rank = dicUnits[type][rank].unitRank;
        unit.unitStat.AttackDelay = dicUnits[type][rank].attackDelay;
        unit.unitStat.AttackRange = dicUnits[type][rank].attackRange;
        unit.unitStat.AttackPoint = dicUnits[type][rank].attackPoint;
        unit.unitStat.AttackType = dicUnits[type][rank].attackType;
        unit.unitStat.Gold = dicUnits[type][rank].unitGold;
    }
}
