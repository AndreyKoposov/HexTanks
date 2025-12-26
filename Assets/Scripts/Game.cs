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

    [SerializeField] private GridManager world;

    private int turn = 1;
    private Team team = Team.Player;

    public static GridManager World
    {
        get => Instance.world;
    }
    public static Team CurrentPlayer
    {
        get => Instance.team;
    }

    public void EndTurn()
    {
        team = (Team)(1 - (int)team);

        GlobalEventManager.OnEndTurn.Invoke(team);

        if (team == Team.Player)
            NextTurn();
    }

    private void NextTurn()
    {
        turn++;

        GlobalEventManager.OnNextTurn.Invoke(turn);
    }
}
