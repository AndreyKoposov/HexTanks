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
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
    #endregion

    [SerializeField] private GridManager world;

    public static GridManager World
    {
        get => Instance.world;
    }
}
