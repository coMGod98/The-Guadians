using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class UnitDB : MonoBehaviour
{
    public static UnitDB instance;

    public TextAsset unitDBFileXml;

    public struct UnitData
    {
        public string unitName;
        public float attackDelay;
        public float attackRange;
        public float attackPoint;
        public int attackType;
        public int unitGold;
    }

    Dictionary<string, UnitData> dicUnits = new Dictionary<string, UnitData>();

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
        //unitXMLDoc.LoadXml(unitDBFileXml.text);

        XmlNodeList unitNodeList = unitXMLDoc.GetElementsByTagName("row");

        foreach (XmlNode unitNode in unitNodeList)
        {
            UnitData unitData = new UnitData();

            foreach (XmlNode childNode in unitNode.ChildNodes)
            {
                if (childNode.Name == "unitName")
                {
                    unitData.unitName = childNode.InnerText;
                }

                if (childNode.Name == "attackDelay")
                {
                    unitData.attackDelay = float.Parse(childNode.InnerText);
                }

                if (childNode.Name == "attackRange")
                {
                    unitData.attackRange = float.Parse(childNode.InnerText);
                }
                if (childNode.Name == "attackPoint")
                {
                    unitData.attackPoint = float.Parse(childNode.InnerText);
                }
                if (childNode.Name == "attackType")
                {
                    unitData.attackType = int.Parse(childNode.InnerText);
                }
                if (childNode.Name == "unitGold")
                {
                    unitData.unitGold = int.Parse(childNode.InnerText);
                }
            }
            dicUnits[unitData.unitName] = unitData;
        }
    }

    public void LoadUnitStatFromXML(string unitName, Unit unit)
    {
       /* unit.unitStat.AttackRange = dicUnits[unitName].attackRange;
        unit.unitStat.AttackPoint = dicUnits[unitName].attackPoint;
        unit.unitStat.AttackType = dicUnits[unitName].attackType;
        unit.unitStat.Gold = dicUnits[unitName].unitGold;*/
    }
}
