using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject unitMarker;

    public void SelectUnit(){
        unitMarker.SetActive(true);
    }
    public void DeselectUnit(){
        unitMarker.SetActive(false);
    }
}
