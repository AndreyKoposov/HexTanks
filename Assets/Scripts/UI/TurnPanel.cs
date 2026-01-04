using TMPro;
using UnityEngine;

public class TurnPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnLabel;
    [SerializeField] private TextMeshProUGUI playerLabel;

    private void Awake()
    {
        RegisterOnEvents();
    }

    private void Start()
    {
        SetTurnLabel(1);
        SetPlayerLabel(Game.CurrentTeam);
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
