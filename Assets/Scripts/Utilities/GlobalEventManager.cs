using UnityEngine;
using UnityEngine.Events;

public static class GlobalEventManager
{
    public static UnityEvent<VectorHex, Team> UnitDied { get; } = new();
    public static UnityEvent<VectorHex, Team> UnitCreated { get; } = new();
    public static UnityEvent<int> TurnChanged { get; } = new();
    public static UnityEvent<Team> EndTurn { get; } = new();
    public static UnityEvent<Transport, int> BoardUnitSelected { get; } = new();
}
