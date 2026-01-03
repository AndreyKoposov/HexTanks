using UnityEngine;

public class ArtManager : MonoBehaviour
{
    [SerializeField] private Material playerMat;
    [SerializeField] private Material enemyMat;

    [SerializeField] private Material playerSphere;
    [SerializeField] private Material enemySphere;

    [SerializeField] private Color selectColor;
    [SerializeField] private Color attackColor;
    [SerializeField] private Color territoryColor;

    public Material PlayerMat => playerMat;
    public Material EnemyMat => enemyMat;
    public Color SelectColor => selectColor;
    public Color AttackColor => attackColor;
    public Color TColor => territoryColor;
    public Material PlayerSphere => playerSphere;
    public Material EnemySphere => enemySphere;
}
