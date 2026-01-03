using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "HexTanks/Unit")]
public class UnitInfo : ScriptableObject
{
    [Header("Unit characteristics")]
    [SerializeField] private UnitType type;
    [SerializeField] private int hp = 0;
    [SerializeField] private int damage = 0;
    [SerializeField] private int minAttackDistance = 0;
    [SerializeField] private int maxAttackDistance = 0;
    [SerializeField] private int movementDistance = 0;
    [SerializeField] private int attackPoints = 0;
    [SerializeField] private bool flying = false;

    [Header("Unit settings")]
    [SerializeField] private float offsetOverTile = 0;
    [SerializeField] private float rotationSpeed = 0;
    [SerializeField] private float moveSpeed = 0;

    [Header("Unit cost")]
    [SerializeField] private float plasm = 0;
    [SerializeField] private float titan = 0;
    [SerializeField] private float chips = 0;

    public int Hp => hp;
    public int Damage => damage;
    public int MinAttackDistance => minAttackDistance;
    public int MaxAttackDistance => maxAttackDistance;
    public int MovementDistance => movementDistance;
    public int AttackPoints => attackPoints;
    public bool Flying => flying;
    public UnitType Type => type;

    public float OffsetOverTile => offsetOverTile;
    public float RotationSpeed => rotationSpeed;
    public float MoveSpeed => moveSpeed;

    public float Plasm => plasm;
    public float Titan => titan;
    public float Chips => chips;
}
