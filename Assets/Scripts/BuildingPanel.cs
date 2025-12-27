using TMPro;
using UnityEngine;

public class BuildingPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnsLeftLabel;
    [SerializeField] private TextMeshProUGUI unitLabel;

    public void Setup(Building building)
    {
        SetTurnsLeftLabel(building.TurnsLeft);
        SetUnitLabel(building.UnitToBuild);
    }

    private void SetTurnsLeftLabel(int turns)
    {
        turnsLeftLabel.text = $"Turn: {turns}";
    }
    private void SetUnitLabel(UnitType type)
    {
        unitLabel.text = $"{type}";
    }
}
