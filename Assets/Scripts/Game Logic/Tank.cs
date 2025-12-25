using UnityEngine;

public class Tank : MonoBehaviour
{
    public int hp = 5;
    public int damage = 2;
    public int attackDistance = 2;
    public int movementDistance = 3;

    public Team team;
    public Vector3Int position;

    public void Setup(Team unitTeam, Vector3Int unitPosition)
    {
        team = unitTeam;
        position = unitPosition;
        transform.localPosition = Vector3.zero - Vector3.forward * 0.09f;
    }

    public void MoveTo(Vector3Int to, bool force = false)
    {
        transform.localPosition = Vector3.zero - Vector3.forward * 0.09f;
    }
}
