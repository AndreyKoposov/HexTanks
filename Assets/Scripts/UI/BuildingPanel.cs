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

    public void Open(Factory factory)
    {
        gameObject.SetActive(true);
        Setup(factory);
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

    private void Setup(Factory factory)
    {
        //SetTurnsLeftLabel(building.TurnsLeft);
        //SetUnitLabel(building.UnitToBuild);

        infantryButton.onClick.AddListener(() => BuildListener(factory, UnitType.Infantry));
        heavyButton.onClick.AddListener(() => BuildListener(factory, UnitType.Heavy));
        scoutButton.onClick.AddListener(() => BuildListener(factory, UnitType.Scout));
        artilleryButton.onClick.AddListener(() => BuildListener(factory, UnitType.Artillery));
        defenderButton.onClick.AddListener(() => BuildListener(factory, UnitType.Defender));
        transportButton.onClick.AddListener(() => BuildListener(factory, UnitType.Transport));
    }
    private void SetTurnsLeftLabel(int turns)
    {
        
    }
    private void SetUnitLabel(UnitType type)
    {
        
    }

    private void BuildListener(Factory factory, UnitType type)
    {
        factory.StartBuildUnit(type);
    }
}
