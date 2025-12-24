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
}
