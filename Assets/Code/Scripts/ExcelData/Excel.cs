using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public enum Type
{
    Warrior,Archer,Wizard
}
public enum Rank
{
    Normal,Rare,Epic,Unique,Legendary
}
public class Excel : MonoBehaviour
{
    public static Excel instance;

    //xml 파일
    public TextAsset unitDBFileXml;

    //여러개의 변수들을 넣어서 구조체 하나를 한개의 상자처럼 간주하고 사용할수 있음
   struct MonParams
    {
        public Type unitType;
        public Rank unitRank;
        public float attackDealy;
        public float attackRange;
        public float attackPoint;
        public int attackType;
        public int unitGold;
    }

   //딕셔너리의 키값으로 적의이름을 사용할 예정이므로 string타입으로 하고 데이터 값으로는 구조체를 이용함 MonParams로 지정
   Dictionary<Type, MonParams> dicMonsters = new Dictionary<Type, MonParams>();
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

    //XML로부터 파라미터 값 읽어 들이기
    void MakeMonsterXML()
    {
        XmlDocument monsterXMLDoc = new XmlDocument();
        monsterXMLDoc.LoadXml(unitDBFileXml.text);

        XmlNodeList monsterNodeList = monsterXMLDoc.GetElementsByTagName("row");

        //노드 리스트로부터 각각의 노드를 뽑아냄
        foreach (XmlNode monsterNode in monsterNodeList)
        {
            MonParams monParams = new MonParams();

            foreach (XmlNode childNode in monsterNode.ChildNodes)
            {
                if (childNode.Name == "unitType")
                {
                    //<name>smallspider</name>
                    monParams.unitType = (Type) Enum.Parse(typeof(Type), childNode.InnerText);
                }

                if (childNode.Name == "unitRank")
                {
                    //<level>1</level>    Int16.Parse() 은 문자열을 정수로 바꿔줌
                    monParams.unitRank = (Rank)Enum.Parse(typeof(Rank),childNode.InnerText);
                }

                if (childNode.Name == "attackDealy")
                {
                    monParams.attackDealy = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "attackRange")
                {
                    monParams.attackRange = Int16.Parse(childNode.InnerText);
                }
                if (childNode.Name == "attackPoint")
                {
                    monParams.attackPoint = Int16.Parse(childNode.InnerText);
                }
                if (childNode.Name == "attackType")
                {
                    monParams.attackType = Int16.Parse(childNode.InnerText);
                }
                if (childNode.Name == "unitGold")
                {
                    monParams.unitGold = Int16.Parse(childNode.InnerText);
                }
                print(childNode.Name + ": " + childNode.InnerText);
            }
            dicMonsters[monParams.unitType] = monParams;
        }
    }

    //외부로부터 몬스터의 이름과, EnemyParams 객체를 전달 받아서 해당 이름을 가진 몬스터의 
    //데이터(XML 에서 읽어 온 데이터)를 전달받은 EnemyParams 객체에 적용하는 역할을 하는 함수
   /* public void LoadMonsterParamsFromXML(string Round,MonsterObject mParams)
    {
        mParams.MonsterHP = dicMonsters[Round].MonsterHP;
        mParams.MonsterGold = mParams.maxHp = dicMonsters[Round].MonsterGold;
        mParams.Movement = dicMonsters[Round].Movement;
        
       
    }*/


    void Update()
    {

    }
}