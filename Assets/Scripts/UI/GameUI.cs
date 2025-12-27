using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnLabel;
    [SerializeField] private TextMeshProUGUI playerLabel;

    [SerializeField] private BuildingPanel buildingPanel;

    private void Awake()
    {
        RegisterOnEvents();
    }
    private void Start()
    {
        SetTurnLabel(1);
        SetPlayerLabel(Game.CurrentPlayer);
    }

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.TurnChanged.AddListener(SetTurnLabel);
        GlobalEventManager.EndTurn.AddListener(SetPlayerLabel);
        GlobalEventManager.BuildingSelected.AddListener(ActivateBuildingPanel);
    }
    private void SetTurnLabel(int turn)
    {
        turnLabel.text = $"Turn: {turn}";
    }
    private void SetPlayerLabel(Team team)
    {
        playerLabel.text = $"{team}";
    }
    private void ActivateBuildingPanel(Building building)
    {
        buildingPanel.gameObject.SetActive(true);
        buildingPanel.Setup(building);
    }
    #endregion
}
