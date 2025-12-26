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

    public static GridManager World
    {
        get => Instance.world;
    }

    public void NextTurn()
    {
        turn++;

        GlobalEventManager.OnNextTurn.Invoke(turn);
    }
}
