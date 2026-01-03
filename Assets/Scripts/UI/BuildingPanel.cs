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

        infantryButton.interactable = true;
        heavyButton.interactable = true;
        scoutButton.interactable = true;
        artilleryButton.interactable = true;
        defenderButton.interactable = true;
        transportButton.interactable = true;
    }

    private void Setup(Factory factory)
    {
        //SetTurnsLeftLabel(building.TurnsLeft);
        //SetUnitLabel(building.UnitToBuild);
        PlayerData player = factory.Team == Team.Player ? Game.Player : Game.Enemy;

        if (player.unitsHas < player.unitsMax)
        {
            infantryButton.onClick.AddListener(() => BuildListener(factory, UnitType.Infantry));
            heavyButton.onClick.AddListener(() => BuildListener(factory, UnitType.Heavy));
            scoutButton.onClick.AddListener(() => BuildListener(factory, UnitType.Scout));
            artilleryButton.onClick.AddListener(() => BuildListener(factory, UnitType.Artillery));
            defenderButton.onClick.AddListener(() => BuildListener(factory, UnitType.Defender));
            transportButton.onClick.AddListener(() => BuildListener(factory, UnitType.Transport));
        }
        else
        {
            infantryButton.interactable = false;
            heavyButton.interactable = false;
            scoutButton.interactable = false;
            artilleryButton.interactable = false;
            defenderButton.interactable = false;
            transportButton.interactable = false;
        }

        if (!player.CanBuildUnit(UnitType.Infantry))
            infantryButton.interactable = false;
        if (!player.CanBuildUnit(UnitType.Heavy))
            heavyButton.interactable = false;
        if (!player.CanBuildUnit(UnitType.Scout))
            scoutButton.interactable = false;
        if (!player.CanBuildUnit(UnitType.Artillery))
            artilleryButton.interactable = false;
        if (!player.CanBuildUnit(UnitType.Defender))
            defenderButton.interactable = false;
        if (!player.CanBuildUnit(UnitType.Transport))
            transportButton.interactable = false;
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
