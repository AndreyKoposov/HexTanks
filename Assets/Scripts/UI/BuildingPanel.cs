using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnsLeftLabel;
    [SerializeField] private TextMeshProUGUI unitLabel;
    [SerializeField] private Button buildButton;

    public void Setup(Building building)
    {
        SetTurnsLeftLabel(building.TurnsLeft);
        SetUnitLabel(building.UnitToBuild);

        buildButton.onClick.RemoveAllListeners();
        buildButton.onClick.AddListener(() => building.StartBuildUnit(UnitType.Tank));
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
