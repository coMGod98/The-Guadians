using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{  
    public enum State{
        Normal, Hold
    }
    public State state;
    public GameObject unitMarker;
    public int seedID;
    public Vector3 destination;
    public Monster target;
}
