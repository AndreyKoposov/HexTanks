using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "HexTanks/Unit")]
public class UnitInfo : ScriptableObject
{
    [SerializeField] private UnitType type;
    [SerializeField] private int hp = 0;
    [SerializeField] private int damage = 0;
    [SerializeField] private int minAttackDistance = 0;
    [SerializeField] private int maxAttackDistance = 0;
    [SerializeField] private int movementDistance = 0;
    [SerializeField] private bool flying = false;

    public int Hp => hp;
    public int Damage => damage;
    public int MinAttackDistance => minAttackDistance;
    public int MaxAttackDistance => maxAttackDistance;
    public int MovementDistance => movementDistance;
    public bool Flying => flying;
    public UnitType Type => type;
}
