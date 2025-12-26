using UnityEngine;

public class HexTile : MonoBehaviour
{
    public VectorHex position;
    public Unit unit;
    public bool isObstacle = false;

    public bool HasUnit
    {
        get { return unit != null; }
    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
        this.unit.SetPosition(this, true);
    }

    public void UnsetUnit()
    {
        unit = null;
    }

    public void SetLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}
