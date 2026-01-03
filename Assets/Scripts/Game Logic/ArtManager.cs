using UnityEngine;

public class ArtManager : MonoBehaviour
{
    [SerializeField] private Material playerMat;
    [SerializeField] private Material enemyMat;
    [SerializeField] private Material neutralMat;

    [SerializeField] private Material playerSphere;
    [SerializeField] private Material enemySphere;

    [SerializeField] private Color selectColor;
    [SerializeField] private Color attackColor;

    [Header("Territory colors")]
    [SerializeField] private Color playerColor;
    [SerializeField] private Color enemyColor;
    [SerializeField] private Color neutralColor;
    [SerializeField] private Color blockedColor;

    public Material PlayerMat => playerMat;
    public Material EnemyMat => enemyMat;
    public Material NeutralMat => neutralMat;
    public Color SelectColor => selectColor;
    public Color AttackColor => attackColor;
    public Material PlayerSphere => playerSphere;
    public Material EnemySphere => enemySphere;

    public Color PlayerColor => playerColor;
    public Color EnemyColor => enemyColor;
    public Color NeutralColor => neutralColor;
    public Color BlockedColor => blockedColor;
}
