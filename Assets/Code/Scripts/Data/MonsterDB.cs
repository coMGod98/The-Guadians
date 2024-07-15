using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MonsterDB : MonoBehaviour
{
    public static MonsterDB instance;

    public TextAsset monsterDBFileXml;

    public struct MonsterData
    {
        public int round;
        public float monsterHP;
        public int monsterGold;
        public float monsterSpeed;
    }

    Dictionary<int, MonsterData> dicMonsters = new Dictionary<int, MonsterData>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        MakeMonsterXML();
    }

    public void MakeMonsterXML()
    {
        XmlDocument monsterXMLDoc = new XmlDocument();
        //monsterXMLDoc.LoadXml(monsterDBFileXml.text);

        XmlNodeList monsterNodeList = monsterXMLDoc.GetElementsByTagName("row");

        foreach (XmlNode monsterNode in monsterNodeList)
        {
            MonsterData monsterData = new MonsterData();

            foreach (XmlNode childNode in monsterNode.ChildNodes)
            {
                if (childNode.Name == "round")
                {
                    monsterData.round = int.Parse(childNode.InnerText);
                }

                if (childNode.Name == "monsterHP")
                {
                    monsterData.monsterHP = float.Parse(childNode.InnerText);
                }

                if (childNode.Name == "monsterGold")
                {
                    monsterData.monsterGold = int.Parse(childNode.InnerText);
                }

                if (childNode.Name == "monsterSpeed")
                {
                    monsterData.monsterSpeed = float.Parse(childNode.InnerText);
                }
            }
            dicMonsters[monsterData.round] = monsterData;
        }
    }

    public void LoadMonsterStatFromXML(string monsterName, Monster monster)
    {
        int round = (int)Char.GetNumericValue(monsterName[^1]);
        
/*        monster.monsterStat.HP = dicMonsters[round].monsterHP;
        monster.monsterStat.Gold = dicMonsters[round].monsterGold;
        monster.monsterStat.Speed = dicMonsters[round].monsterSpeed;*/
    }
}
