using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TurnPanel turnPanel;
    [SerializeField] private BuildingPanel buildingPanel;
    [SerializeField] private Button defenderButton;
    [SerializeField] private TransportPanel transportPanel;

    public void OpenBuildingPanel(Factory building)
    {
        buildingPanel.Open(building);
    }
    public void CloseBuildingPanel()
    {
        buildingPanel.Close();
    }

    public void ShowDefenderButton(Defender unit)
    {
        defenderButton.gameObject.SetActive(true);
        defenderButton.interactable = unit.CanActivateField;

        defenderButton.onClick.AddListener(() => unit.SetField(!unit.FieldActive));
    }
    public void HideDefenderButton()
    {
        defenderButton.gameObject.SetActive(false);

        defenderButton.onClick.RemoveAllListeners();
    }
    public void OpenTrasportPanel(Transport transport)
    {
        transportPanel.Open(transport);
    }
    public void CloseTransportPanel()
    {
        transportPanel.Close();
    }
}
