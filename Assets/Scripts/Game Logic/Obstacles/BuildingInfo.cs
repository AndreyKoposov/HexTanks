using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "HexTanks/Building")]
public class BuildingInfo : ScriptableObject
{
    [SerializeField] private int unitsContain;

    [Header("Production")]
    [SerializeField] private int producePlasm;
    [SerializeField] private int produceTitan;
    [SerializeField] private int produceChips;

    public int UnitsContain => unitsContain;
    public int ProducePlasm => producePlasm;
    public int ProduceTitan => produceTitan;
    public int ProduceChips => produceChips;
}
