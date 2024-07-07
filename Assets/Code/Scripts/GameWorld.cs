using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagers : Singleton<GameManagers>
{
    [SerializeField] private UnitManager _unitManager;
    [SerializeField] private MonsterManager _monsterManager;
    [SerializeField] private InputManager _inputManager;

    public UnitManager UnitManager => _unitManager;
    public MonsterManager MonsterManager => _monsterManager;
    public InputManager InputManager => _inputManager;
    private void Update(){
        _inputManager.AdvanceInput();
    }
}
