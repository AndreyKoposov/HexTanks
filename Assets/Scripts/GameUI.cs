using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnLabel;

    private void Awake()
    {
        RegisterEvents();
    }

    #region Events
    private void RegisterEvents()
    {
        GlobalEventManager.OnNextTurn.AddListener(SetTurnLabel);
    }
    private void SetTurnLabel(int turn)
    {
        turnLabel.text = $"Turn: {turn}";
    }
    #endregion
}
