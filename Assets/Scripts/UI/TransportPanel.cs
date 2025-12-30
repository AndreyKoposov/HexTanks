using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransportPanel : MonoBehaviour
{
    public List<Button> buttons = new();

    public void Open(Transport transport)
    {
        gameObject.SetActive(true);

        for (int i = 0; i < transport.units.Count; i++)
        {
            var unit = transport.units[i];

            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{unit.Info.Type}";
            buttons[i].onClick.AddListener(() => GlobalEventManager.BoardUnitSelected.Invoke(transport, unit));
        }
    }
    public void Close()
    {
        foreach (var button in buttons)
            button.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}
