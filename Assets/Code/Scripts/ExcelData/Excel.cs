using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
public class Excel : MonoBehaviour
{
    public static Excel instance;

    //xml 파일
    public TextAsset enemyFileXml;

    //여러개의 변수들을 넣어서 구조체 하나를 한개의 상자처럼 간주하고 사용할수 있음
    struct MonParams
    {
        //xml 파일로 부터 각각의 몬스터의 대해서 이들 파라미터 값을 읽어 들이고 구조체 내부 변수에 저장하고 구조체를 이용하여 각 몬스터에게 파라미터 값으 전달함
        public string Round;
        public int MonsterHP;
        public int MonsterGold;
        public int Movement;
    }

    //딕셔너리의 키값으로 적의이름을 사용할 예정이므로 string타입으로 하고 데이터 값으로는 구조체를 이용함 MonParams로 지정
    Dictionary<string, MonParams> dicMonsters = new Dictionary<string, MonParams>();
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
        monsterXMLDoc.LoadXml(enemyFileXml.text);

        XmlNodeList monsterNodeList = monsterXMLDoc.GetElementsByTagName("row");

        //노드 리스트로부터 각각의 노드를 뽑아냄
        foreach (XmlNode monsterNode in monsterNodeList)
        {
            MonParams monParams = new MonParams();

            foreach (XmlNode childNode in monsterNode.ChildNodes)
            {
                if (childNode.Name == "Round")
                {
                    //<name>smallspider</name>
                    monParams.Round = childNode.InnerText;
                }

                if (childNode.Name == "MonsterHP")
                {
                    //<level>1</level>    Int16.Parse() 은 문자열을 정수로 바꿔줌
                    monParams.MonsterHP = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "MonsterGold")
                {
                    monParams.MonsterGold = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "Movement")
                {
                    monParams.Movement = Int16.Parse(childNode.InnerText);
                }              
                print(childNode.Name + ": " + childNode.InnerText);
            }
            dicMonsters[monParams.Round] = monParams;
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