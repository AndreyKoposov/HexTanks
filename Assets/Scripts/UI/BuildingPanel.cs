using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanel : MonoBehaviour
{
    [SerializeField] private Button infantryButton;
    [SerializeField] private Button heavyButton;
    [SerializeField] private Button scoutButton;
    [SerializeField] private Button artilleryButton;
    [SerializeField] private Button defenderButton;
    [SerializeField] private Button transportButton;

    public void Open(Building building)
    {
        gameObject.SetActive(true);
        Setup(building);
    }
    public void Close()
    {
        gameObject.SetActive(false);

        infantryButton.onClick.RemoveAllListeners();
        heavyButton.onClick.RemoveAllListeners();
        scoutButton.onClick.RemoveAllListeners();
        artilleryButton.onClick.RemoveAllListeners();
        defenderButton.onClick.RemoveAllListeners();
        transportButton.onClick.RemoveAllListeners();
    }

    private void Setup(Building building)
    {
        //SetTurnsLeftLabel(building.TurnsLeft);
        //SetUnitLabel(building.UnitToBuild);

        infantryButton.onClick.AddListener(() => BuildListener(building, UnitType.Infantry));
        heavyButton.onClick.AddListener(() => BuildListener(building, UnitType.Heavy));
        scoutButton.onClick.AddListener(() => BuildListener(building, UnitType.Scout));
        artilleryButton.onClick.AddListener(() => BuildListener(building, UnitType.Artillery));
        defenderButton.onClick.AddListener(() => BuildListener(building, UnitType.Defender));
        transportButton.onClick.AddListener(() => BuildListener(building, UnitType.Transport));
    }
    private void SetTurnsLeftLabel(int turns)
    {
        
    }
    private void SetUnitLabel(UnitType type)
    {
        
    }

    private void BuildListener(Building building, UnitType type)
    {
        building.StartBuildUnit(type);
    }
}
