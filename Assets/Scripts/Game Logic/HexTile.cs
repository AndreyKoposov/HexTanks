using UnityEngine;

public class HexTile : MonoBehaviour
{
    public VectorHex position;
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
        tank.MoveTo(position, true);
    }

    public void UnsetUnit()
    {
        tank = null;
    }

    public void SetLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}
