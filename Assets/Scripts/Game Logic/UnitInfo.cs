using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Unit")]
public class UnitInfo : ScriptableObject
{
    [SerializeField] private int hp = 0;
    [SerializeField] private int damage = 0;
    [SerializeField] private int minAttackDistance = 0;
    [SerializeField] private int maxAttackDistance = 0;
    [SerializeField] private int movementDistance = 0;
    [SerializeField] private bool flying = false;
    [SerializeField] private GameObject prefab;

    public int Hp => hp;
    public int Damage => damage;
    public int MinAttackDistance => minAttackDistance;
    public int MaxAttackDistance => maxAttackDistance;
    public int MovementDistance => movementDistance;
    public bool Flying => flying;
    public bool Prefab => prefab;
}
