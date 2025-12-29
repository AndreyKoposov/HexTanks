using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected static Vector3 OffsetOverTile = Vector3.up * 0.09f;

    public UnitInfo info;

    protected int hp;
    protected Team team;
    protected VectorHex position;

    protected int movePoints;
    protected int attackPoints;

    private VectorHex animationPos;

    public bool Dead => hp <= 0;
    public bool CanMove => movePoints > 0 && attackPoints == info.AttackPoints;
    public bool CanAttack => attackPoints > 0 && movePoints == info.MovementDistance;

    public Team Team => team;
    public VectorHex Position => position;
    public int MovePoints => movePoints;

    private void Awake()
    {
        RegisterOnEvents();
    }

    public void Setup(Team team)
    {
        SetTeam(team);

        hp = info.Hp;
        movePoints = info.MovementDistance;
        attackPoints = info.AttackPoints;

        transform.rotation = Quaternion.identity;
    }

    #region Actions
    public virtual void MoveTo(HexTile to, bool spawn)
    {
        if (spawn)
            SetParent(to);
        else
        {
            List<VectorHex> path = A_Star.FindShortestPath(position, to.Position);
            StartCoroutine(MoveByPath(path));

            movePoints -= path.Count;
        }

        position = to.Position;
    }
    public virtual void AttackUnit(Unit attacked)
    {
        attacked.DealDamage(info.Damage);

        attackPoints--;
    }
    public void DealDamage(int damage)
    {
        hp -= damage;
    }
    #endregion

    #region Operations
    protected virtual void SetParent(HexTile to)
    {
        //gameObject.transform.parent = to.gameObject.transform;
        transform.position = to.gameObject.transform.position;
        transform.position += OffsetOverTile;
    }
    protected void SetTeam(Team team)
    {
        this.team = team;

        //if (team == Team.Player)
        //    GetComponent<MeshRenderer>().material = Game.Art.PlayerMat;
        //if (team == Team.Enemy)
        //    GetComponent<MeshRenderer>().material = Game.Art.EnemyMat;
    }
    protected IEnumerator MoveByPath(List<VectorHex> path)
    {
        animationPos = position;
        foreach (VectorHex p in path)
        {
            yield return Rotate(p);
            yield return MoveByLine(Game.Grid[p].transform.position);
            SetParent(Game.Grid[p]);
            animationPos = p;
        }
    }
    protected IEnumerator Rotate(VectorHex p)
    {
        int frames = 20;

        var tilePos = Game.Grid[p].transform.position;

        Vector3 lookDirection = transform.forward;
        Vector3 targetDirection = (new Vector3(tilePos.x, transform.position.y, tilePos.z) - transform.position).normalized;
        float targetAngle = Vector3.SignedAngle(lookDirection, targetDirection, Vector3.up);

        if (Mathf.Abs(targetAngle) > 0.01f)
            for (int j = 0; j < frames; j++)
            {
                transform.Rotate(Vector3.up, targetAngle / frames);
                yield return new WaitForSeconds(0.2f / frames);
            }
    }
    protected IEnumerator MoveByLine(Vector3 point)
    {
        int frames = 20;
        float delta = (transform.position - point).magnitude;
        for (int j = 0; j < frames; j++)
        {
            transform.Translate(delta / frames * Vector3.forward, Space.Self);
            yield return new WaitForSeconds(0.3f / frames);
        }
    }
    #endregion

    #region Events
    protected void RegisterOnEvents()
    {
        GlobalEventManager.TurnChanged.AddListener(ResetOnTurnChanged);
    }
    protected void ResetOnTurnChanged(int _)
    {
        movePoints = info.MovementDistance;
        attackPoints = info.AttackPoints;
    }
    #endregion
}
