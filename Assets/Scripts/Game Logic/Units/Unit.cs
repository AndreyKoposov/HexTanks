using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitInfo info;

    private int hp;
    private Team team;
    private VectorHex position;
    private bool canMove;

    public bool Dead => hp <= 0;
    public bool CanMove => canMove;
    public Team Team => team;

    private void Awake()
    {
        RegisterOnEvents();
    }

    public void Setup(Team Team)
    {
        team = Team;
        hp = info.Hp;
        canMove = true;
        transform.rotation = Quaternion.identity;

        if (team == Team.Player)
            GetComponent<MeshRenderer>().material = Game.Instance.playerMat;
        if (team == Team.Enemy)
            GetComponent<MeshRenderer>().material = Game.Instance.enemyMat;
    }

    #region Actions
    public void MoveTo(HexTile to, bool force = false)
    {
        //Debug.Log($"From = {position}, To = {to.position}");
        List<VectorHex> path = A_Star.FindShortestPath(position, to.position);
        //if (path == null)
        //    Debug.Log("Empty path!!!");
        //else
        //{
        //    Debug.Log(path.Count);
        //    foreach (var pos in path)
        //        Debug.DrawRay(Game.World[pos].gameObject.transform.position, new(0, 5, 0), Color.red, 5f);
        //}
        canMove = false;
        if (force)
            SetPosition(to);
        else
        {
            StartCoroutine(MoveByPath(path));
        }
    }
    public void AttackUnit(Unit attacked)
    {
        attacked.DealDamage(info.Damage);

        canMove = false;
    }
    #endregion

    #region Operations
    public void SetPosition(HexTile to)
    {
        position = to.position;

        gameObject.transform.parent = to.gameObject.transform;
        transform.localPosition = Vector3.zero - Vector3.forward * 0.09f;
    }
    public void DealDamage(int damage)
    {
        hp -= damage;
    }
    #endregion

    private IEnumerator MoveByPath(List<VectorHex> path)
    {
        foreach (VectorHex p in path)
        {
            SetPosition(Game.World[p]);
            yield return new WaitForSeconds(0.5f);
        }
    }

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
