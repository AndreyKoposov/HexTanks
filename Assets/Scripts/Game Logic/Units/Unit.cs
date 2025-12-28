using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Unit : MonoBehaviour
{
    private static Vector3 OffsetOverTile = Vector3.zero - Vector3.forward * 0.09f;

    public UnitInfo info;

    private int hp;
    private Team team;
    private VectorHex position;
    private bool canMove;

    private VectorHex currentDirection = VectorHex.UNSIGNED;

    public bool Dead => hp <= 0;
    public bool CanMove => canMove;
    public Team Team => team;

    private void Awake()
    {
        RegisterOnEvents();
    }

    public void Setup(Team team)
    {
        SetTeam(team);
        hp = info.Hp;
        canMove = true;
        transform.rotation = Quaternion.identity;
    }

    #region Actions
    public void MoveTo(HexTile to, bool spawn)
    {
        if (spawn)
            SetPosition(to);
        else
        {
            List<VectorHex> path = A_Star.FindShortestPath(position, to.Position);
            StartCoroutine(MoveByPath(path));

            canMove = false;
        }
    }
    public void AttackUnit(Unit attacked)
    {
        attacked.DealDamage(info.Damage);

        canMove = false;
    }
    public void DealDamage(int damage)
    {
        hp -= damage;
    }
    #endregion

    #region Operations
    private void SetPosition(HexTile to)
    {
        position = to.Position;

        gameObject.transform.parent = to.gameObject.transform;
        transform.localPosition = OffsetOverTile;
    }
    private void SetTeam(Team team)
    {
        this.team = team;

        if (team == Team.Player)
            GetComponent<MeshRenderer>().material = Game.Art.PlayerMat;
        if (team == Team.Enemy)
            GetComponent<MeshRenderer>().material = Game.Art.EnemyMat;
    }
    private IEnumerator MoveByPath(List<VectorHex> path)
    {
        foreach (VectorHex p in path)
        {
            yield return Rotate(p);
            yield return MoveByLine(Game.Grid[p].transform.position);
            SetPosition(Game.Grid[p]);
        }
    }
    private IEnumerator Rotate(VectorHex p)
    {
        int frames = 20;
        float targetAngle = 0;

        if (p == position + position.Left)
            targetAngle = 0;
        if (p == position + position.LeftBottom)
            targetAngle = -60;
        if (p == position + position.RightBottom)
            targetAngle = -120;
        if (p == position + position.Right)
            targetAngle = -180;
        if (p == position + position.RightTop)
            targetAngle = -240;
        if (p == position + position.LeftTop)
            targetAngle = -300;

        var delta = targetAngle - transform.rotation.eulerAngles.y;

        if (Mathf.Abs(delta) > Mathf.Abs(delta + 360))
            delta += 360;

        if (Mathf.Abs(targetAngle - transform.rotation.eulerAngles.y) > 0.00001f)
            for (int j = 0; j < frames; j++)
            {
                transform.Rotate(Vector3.up, delta / frames);
                yield return new WaitForSeconds(0.25f / frames);
            }
    }
    private IEnumerator MoveByLine(Vector3 point)
    {
        int frames = 20;
        float delta = (transform.position - point).magnitude;
        for (int j = 0; j < frames; j++)
        {
            transform.Translate(delta / frames * Vector3.left, Space.Self);
            yield return new WaitForSeconds(0.5f / frames);
        }
    }
    #endregion

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.TurnChanged.AddListener(ResetOnTurnChanged);
    }
    private void ResetOnTurnChanged(int _)
    {
        canMove = true;
    }
    #endregion
}
