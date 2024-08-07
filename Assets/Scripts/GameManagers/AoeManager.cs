using EasyUI.Toast;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class AoeManager : MonoBehaviour
{
    [Header("aoe Prefab")]
    public GameObject[] aoePrefabs;

    [Header("range Prefab")]
    public GameObject rangePrefab;

    private GameObject curRangeCheck;

    private bool isPlacingAOE = false;
    private int aoeIndex = 0;
    private int placeableMask;

    public event Action AoePlaced;

    public bool MeteoActive = false;

    void Start()
    {
        placeableMask = LayerMask.GetMask("Ground");
    }

    public void Update()
    {
        if (isPlacingAOE)
        {
            RangeofAOE();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceAOE();
            }
        }
    }

    public void ButtonClicked()
    {
        isPlacingAOE = true;
        if (curRangeCheck == null)
        {
            curRangeCheck = Instantiate(rangePrefab);
        }
        curRangeCheck.SetActive(true);
    }

    public void RangeofAOE()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placeableMask) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Vector3 indicatorPosition = new Vector3(hit.point.x, 0.7f, hit.point.z);
            curRangeCheck.transform.position = indicatorPosition;
            curRangeCheck.SetActive(true);
        }
    }

    public void PlaceAOE()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placeableMask) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Vector3 towerPosition = new Vector3(hit.point.x, 0.7f, hit.point.z);
            GameObject aoeInstance = Instantiate(aoePrefabs[aoeIndex], towerPosition, Quaternion.identity);
            aoeInstance.GetComponent<AudioSource>().Play();
            isPlacingAOE = false;
            curRangeCheck.SetActive(false);
            AoePlaced?.Invoke();
            GameWorld.Instance.UIManager.isButtonLocked = false;
            AoeSkills(aoePrefabs[aoeIndex], towerPosition);
            Destroy(aoeInstance, 5.0f); 
        }
    }

    public void SelectedAOE(int index)
    {
        if (index >= 0 && index < aoePrefabs.Length)
        {
            aoeIndex = index;
        }
    }

    public void AoeSkills(GameObject aoePrefab, Vector3 position)
    {
        if (aoePrefab == aoePrefabs[0])
        {
            if(MeteoActive)
            {
                return;
            }
            StartCoroutine(MeteoAOE(position));
        }
        else if (aoePrefab == aoePrefabs[1])
        {
            StartCoroutine(SnowAOE(position));
        }
        else if (aoePrefab == aoePrefabs[2])
        {
            StartCoroutine(BomBAOE(position));
        }
    }

    private IEnumerator MeteoAOE(Vector3 position)
    {
        MeteoActive = true;

        float duration = 5.0f; // 지속시간
        float damageInterval = 0.5f; // 데미지간격
        float damageAmount = 50.0f; // damageinterval 당 데미지 양
        float time = 0.0f;

        while (time < duration)
        {
            Collider[] colliders = Physics.OverlapSphere(position, 5.5f, LayerMask.GetMask("Monster"));
            foreach (Collider col in colliders)
            {
                Monster monster = col.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.InflictDamage(damageAmount);
                }
            }
            time += damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }

        MeteoActive = false;
    }

    private IEnumerator SnowAOE(Vector3 position)
    {
        float duration = 5.0f; // 지속시간
        float checkInterval = 0.5f; // 체크간격
        float slowSpeed = 1.0f; // 느려졌을때 속도
        List<Monster> slowedMonsters = new List<Monster>();
        float time = 0.0f;

        while (time < duration)
        {
            Collider[] colliders = Physics.OverlapSphere(position, 5.5f, LayerMask.GetMask("Monster"));
            foreach (Collider col in colliders)
            {
                Monster monster = col.GetComponent<Monster>();
                if (monster != null && !slowedMonsters.Contains(monster))
                {
                    slowedMonsters.Add(monster);
                    monster.monsterData.Speed = slowSpeed;
                }
            }

            for (int i = slowedMonsters.Count - 1; i >= 0; i--)
            {
                if (Vector3.Distance(slowedMonsters[i].transform.position, position) > 5.5f)
                {
                    slowedMonsters[i].monsterData.Speed = GameWorld.Instance.BalanceManager.monsterDic[slowedMonsters[i].monsterData.round].Speed; 
                    slowedMonsters.RemoveAt(i);
                }
            }

            time += checkInterval;
            yield return new WaitForSeconds(checkInterval);
        }

        foreach (Monster monster in slowedMonsters)
        {
            monster.monsterData.Speed = GameWorld.Instance.BalanceManager.monsterDic[monster.monsterData.round].Speed; 
        }
    }


    private IEnumerator BomBAOE(Vector3 position)
    {
        float radius = 5.0f;
        yield return new WaitForSeconds(0.9f);

        Collider[] colliders = Physics.OverlapSphere(position, radius, LayerMask.GetMask("Monster"));
        foreach (Collider col in colliders)
        {
            Monster monster = col.GetComponent<Monster>();
            if (monster != null && Vector3.Distance(monster.transform.position, position) <= radius)
            {
                monster.InflictDamage(500.0f); // 데미지
            }
        }
    }



    public void ButtonClick(int index)
    {
        SelectedAOE(index);
        ButtonClicked();
    }
}
