using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Vector3Int position;
    public Tank tank;
    public bool isObstacle = false;

    public bool HasUnit
    {
        get { return tank != null; }
    }

    public void SetUnit(Tank unit)
    {
        tank = unit;
        tank.gameObject.transform.parent = transform;
    }

    public void UnsetUnit()
    {
        tank = null;
    }
}
