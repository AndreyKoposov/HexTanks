using UnityEngine;

public class ArtManager : MonoBehaviour
{
    [SerializeField] private Material playerMat;
    [SerializeField] private Material enemyMat;

    [SerializeField] private Color selectColor;
    [SerializeField] private Color attackColor;

    public Material PlayerMat => playerMat;
    public Material EnemyMat => enemyMat;
    public Color SelectColor => selectColor;
    public Color AttackColor => attackColor;
}
