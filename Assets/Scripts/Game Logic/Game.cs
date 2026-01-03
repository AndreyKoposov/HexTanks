using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    #region Singleton
    public static Game Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && this != Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    [SerializeField] private GridManager grid;
    [SerializeField] private UIManager ui;
    [SerializeField] private ArtManager art;
    [SerializeField] private UnitFabric fabric;

    private int turn = 1;
    private Team team = Team.Player;
    private Dictionary<Team, PlayerData> players = new();
    private PlayerData player = new (Team.Player);
    private PlayerData enemy = new(Team.Enemy);

    public static GridManager Grid
    {
        get => Instance.grid;
    }
    public static UIManager UI
    {
        get => Instance.ui;
    }
    public static ArtManager Art
    {
        get => Instance.art;
    }
    public static UnitFabric Fabric
    {
        get => Instance.fabric;
    }

    public static Team CurrentPlayer
    {
        get => Instance.team;
    }
    public static PlayerData Player => Instance.player;
    public static PlayerData Enemy => Instance.enemy;

    public void EndTurn()
    {
        team = (Team)(1 - (int)team);

        GlobalEventManager.EndTurn.Invoke(team);

        if (team == Team.Player)
            NextTurn();
    }

    private void NextTurn()
    {
        turn++;

        GlobalEventManager.TurnChanged.Invoke(turn);
    }
}
