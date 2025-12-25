using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitInfo info;

    private int hp;
    private Team team;
    private VectorHex position;

    public void Setup(Team Team)
    {
        team = Team;
        hp = info.Hp;
        transform.rotation = Quaternion.identity;
    }

    public void MoveTo(HexTile to, bool force = false)
    {
        position = to.position;

        gameObject.transform.parent = to.gameObject.transform;
        transform.localPosition = Vector3.zero - Vector3.forward * 0.09f;
    }
}
