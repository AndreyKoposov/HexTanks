using UnityEngine;
using UnityEngine.Events;

public static class GlobalEventManager
{
    public static UnityEvent<Vector3Int> OnUnitCreated { get; } = new();
    public static UnityEvent<Vector3Int> OnUnitDestroyed { get; } = new();
    public static UnityEvent<int> OnNextTurn { get; } = new();
    public static UnityEvent<Team> OnEndTurn { get; } = new();
}
