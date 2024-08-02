using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingGetGold : MonoBehaviour
{
    public Transform floatPlace;
    public GameObject goldPrefab;
    
    public void FloatGold(List<Unit> selectedUnitList)
    {
        int totalGold = 0;
        foreach(Unit unit in selectedUnitList)
        {
            totalGold += unit.unitData.salesGold;
        }
        GameObject gold = Instantiate(goldPrefab, floatPlace);
        gold.GetComponentInChildren<TextMeshProUGUI>().text =  $"+{totalGold}Gold";
        StartCoroutine(FloatingGold(gold));
    }

    public void FloatGold(Monster monster)
    {
        GameObject gold = Instantiate(goldPrefab, floatPlace);
        gold.GetComponentInChildren<TextMeshProUGUI>().text =  $"+{monster.monsterData.Gold}Gold";
        StartCoroutine(FloatingGold(gold));
    }

    IEnumerator FloatingGold(GameObject gold)
    {
        float dist = 0.0f;
        float moveSpeed = 30.0f;
        Vector3 moveDir = Vector3.up;
        while (dist < 100.0f)
        {
            gold.transform.localPosition += moveDir * moveSpeed * Time.deltaTime;
            dist += moveSpeed * Time.deltaTime;

            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        Destroy(gold);
    }
}
