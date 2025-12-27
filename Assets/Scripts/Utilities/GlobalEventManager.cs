using UnityEngine;
using UnityEngine.Events;

public static class GlobalEventManager
{
    public static UnityEvent<Vector3Int> UnitCreated { get; } = new();
    public static UnityEvent<Vector3Int> UnitDestroyed { get; } = new();
    public static UnityEvent<int> TurnChanged { get; } = new();
    public static UnityEvent<Team> EndTurn { get; } = new();
}
