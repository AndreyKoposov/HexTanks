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

    public void OpenBuildingPanel(Building building)
    {
        buildingPanel.gameObject.SetActive(true);
        buildingPanel.Setup(building);
    }
    public void CloseBuildingPanel()
    {
        buildingPanel.gameObject.SetActive(false);
    }

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.TurnChanged.AddListener(SetTurnLabel);
        GlobalEventManager.EndTurn.AddListener(SetPlayerLabel);
    }
    private void SetTurnLabel(int turn)
    {
        turnLabel.text = $"Turn: {turn}";
    }
    private void SetPlayerLabel(Team team)
    {
        playerLabel.text = $"{team}";
    }
    #endregion
}
