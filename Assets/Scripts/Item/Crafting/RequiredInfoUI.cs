using TMPro;
using UnityEngine;

public class RequiredInfoUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;

    public void Set(string name, int owned, int required)
    {
        Debug.Log($"🧪 Set() 진입: {name}, {owned}/{required}");
        nameText.text = name;
        amountText.text = $"{owned}/{required}";
        amountText.color = (owned < required) ? Color.red : Color.white;
        Debug.Log($"[RequiredInfoUI] nameText: {nameText.text}, amountText: {amountText.text}, color: {amountText.color}");
    }
}
