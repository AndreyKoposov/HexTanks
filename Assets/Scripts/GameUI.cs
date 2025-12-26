using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
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
        SetPlayerLabel(Game.CurrentPlayer);
    }

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.OnNextTurn.AddListener(SetTurnLabel);
        GlobalEventManager.OnEndTurn.AddListener(SetPlayerLabel);
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
