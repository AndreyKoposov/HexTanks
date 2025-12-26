using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnLabel;

    private void Awake()
    {
        RegisterOnEvents();
    }

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.OnNextTurn.AddListener(SetTurnLabel);
    }
    private void SetTurnLabel(int turn)
    {
        turnLabel.text = $"Turn: {turn}";
    }
    #endregion
}
