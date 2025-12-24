using UnityEngine;
using UnityEngine.Events;

public static class GlobalEventManager
{
    public static UnityEvent<Team, Vector3Int> OnUnitCreated = new();
    public static UnityEvent<Team, Vector3Int> OnUnitDestroyed = new();
}
