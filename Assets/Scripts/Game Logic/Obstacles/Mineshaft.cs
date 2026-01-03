using UnityEngine;

public class Mineshaft : Building
{
    private void Awake()
    {
        RegisterOnEvents();
    }

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.TurnChanged.AddListener(ProduceOnTurnChanged);
    }
    private void ProduceOnTurnChanged(int _)
    {
        Debug.Log($"Produce {info.ProducePlasm}");
        Debug.Log($"Produce {info.ProduceTitan}");
        Debug.Log($"Produce {info.ProduceChips}");
    }
    #endregion
}
