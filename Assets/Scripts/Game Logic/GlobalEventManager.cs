using UnityEngine;
using UnityEngine.Events;

public static class GlobalEventManager
{
    public static UnityEvent<Vector3Int> OnUnitCreated = new();
    public static UnityEvent<Vector3Int> OnUnitDestroyed = new();
}
