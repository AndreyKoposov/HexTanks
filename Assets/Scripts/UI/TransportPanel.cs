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

        for (int i = 0; i < transport.Count; i++)
        {
            var unit = transport[i];
            var unitIndex = i;

            if (unit == null)
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
            else
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{unit.Info.Type}";
                buttons[i].onClick.AddListener(() => GlobalEventManager.BoardUnitSelected.Invoke(transport, unitIndex));
            }
        }
    }
    public void Close()
    {
        foreach (var button in buttons)
            button.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}
