using UnityEngine;
using UnityEngine.Events;

public static class GlobalEventManager
{
    public static UnityEvent<VectorHex> UnitDied { get; } = new();
    public static UnityEvent<int> TurnChanged { get; } = new();
    public static UnityEvent<Team> EndTurn { get; } = new();
}
