using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameWorld : Singleton<GameWorld>
{
    [SerializeField] private UnitManager _unitManager;
    [SerializeField] private MonsterManager _monsterManager;
    [SerializeField] private InputManager _inputManager;

    public UnitManager UnitManager => _unitManager;
    public MonsterManager MonsterManager => _monsterManager;
    public InputManager InputManager => _inputManager;

    private float RoundTime = 0.0f;

    private void Awake(){

    }
    private void Update(){
        RoundTime += Time.deltaTime;

        _inputManager.AdvanceInput();
        if(RoundTime > 2.0f){
            _monsterManager.SpawnMonster();
            RoundTime = 0.0f;
        }

        _unitManager.Move();
        _monsterManager.Move();
    }
}
