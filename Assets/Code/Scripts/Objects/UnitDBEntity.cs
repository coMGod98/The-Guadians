using UnityEngine;

public enum UnitType{
    Warrior, Archer, Wizard
}

public enum UnitRank{
    Noraml, Rare, Epic, Unique, Legendary
}


[System.Serializable]
public class UnitDBEntity : MonoBehaviour
{
    [SerializeField] private UnitType _unitType;
    [SerializeField] private UnitRank _unitRank;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackPoint;
    [SerializeField] private float _attackType;

    // public int ID => _id;
    // public UnitType UnitType => _unitType;
    // public UnitRank UnitRank => _unitRank;
    // public float AttackDelay => _attackDelay;
    // public float AttackRange => _attackRange;
    // public float AttackPoint => _attackPoint;
    // public float AttackType => _attackType;

}
