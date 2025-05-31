using TMPro;
using UnityEngine;

public class RequiredInfoUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;

    public void Set(string name, int owned, int required)
    {
        nameText.text = name;
        amountText.text = $"{owned}/{required}";
        amountText.color = (owned < required) ? Color.red : Color.white;
    }
}
