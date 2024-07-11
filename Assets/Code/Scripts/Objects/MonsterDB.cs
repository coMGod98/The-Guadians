using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MonsterDB : MonoBehaviour
{
    public static MonsterDB instance;

    public TextAsset monsterDBFileXml;

    public struct MonsterStat
    {
        public int round;
        public float monsterHP;
        public int monsterGold;
        public float monsterSpeed;
    }

    Dictionary<int, MonsterStat> dicMonsters = new Dictionary<int, MonsterStat>();

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
        monsterXMLDoc.LoadXml(monsterDBFileXml.text);

        XmlNodeList monsterNodeList = monsterXMLDoc.GetElementsByTagName("row");

        foreach (XmlNode monsterNode in monsterNodeList)
        {
            MonsterStat monsterStat = new MonsterStat();

            foreach (XmlNode childNode in monsterNode.ChildNodes)
            {
                if (childNode.Name == "round")
                {
                    monsterStat.round = int.Parse(childNode.InnerText);
                }

                if (childNode.Name == "monsterHP")
                {
                    monsterStat.monsterHP = float.Parse(childNode.InnerText);
                }

                if (childNode.Name == "monsterGold")
                {
                    monsterStat.monsterGold = int.Parse(childNode.InnerText);
                }

                if (childNode.Name == "monsterSpeed")
                {
                    monsterStat.monsterSpeed = float.Parse(childNode.InnerText);
                }
            }
            dicMonsters[monsterStat.round] = monsterStat;
        }
    }

    public void LoadMonsterStatFromXML(string monsterName, Monster monster)
    {
        int round = (int)Char.GetNumericValue(monsterName[^1]);

        monster.monsterStat.Round = dicMonsters[round].round;
        monster.monsterStat.HP = dicMonsters[round].monsterHP;
        monster.monsterStat.Gold = dicMonsters[round].monsterGold;
        monster.monsterStat.Speed = dicMonsters[round].monsterSpeed;
    }
}
