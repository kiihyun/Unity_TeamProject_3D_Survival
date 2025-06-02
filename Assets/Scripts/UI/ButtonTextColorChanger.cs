using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonTextColorChanger : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public TMP_Text text;
    public Color selectedColor = Color.yellow;
    public Color defaultColor = Color.white;

    public void OnSelect(BaseEventData eventData)
    {
        text.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = defaultColor;
    }
}