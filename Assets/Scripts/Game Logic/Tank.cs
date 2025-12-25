using UnityEngine;

public class Tank : MonoBehaviour
{
    public int hp = 5;
    public int damage = 2;
    public int attackDistance = 2;
    public int movementDistance = 3;

    public Team team;
    public VectorHex position;

    public void MoveTo(VectorHex to, bool force = false)
    {
        position = to;
        transform.localPosition = Vector3.zero - Vector3.forward * 0.09f;
    }
}
