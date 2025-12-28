using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TurnPanel turnPanel;
    [SerializeField] private BuildingPanel buildingPanel;

    public void OpenBuildingPanel(Building building)
    {
        buildingPanel.Open(building);
    }
    public void CloseBuildingPanel()
    {
        buildingPanel.Close();
    }
}
