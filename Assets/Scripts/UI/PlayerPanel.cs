using TMPro;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitsLabel;
    [SerializeField] private TextMeshProUGUI plasmLabel;
    [SerializeField] private TextMeshProUGUI titanLabel;
    [SerializeField] private TextMeshProUGUI chipsLabel;

    public void UpdateLabels(PlayerData data)
    {
        if (data.team == Team.Enemy) return;

        unitsLabel.text = $"{data.unitsHas}/{data.unitsMax}";
        plasmLabel.text = $"{data.plasm}+{data.plasmProd}";
        titanLabel.text = $"{data.titan}+{data.titanProd}"; 
        chipsLabel.text = $"{data.chips}+{data.chipsProd}";
    }
}
