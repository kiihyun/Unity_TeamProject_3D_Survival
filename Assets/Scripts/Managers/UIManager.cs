using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject gameplayUI;
    public GameObject dialogueUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowDialogueUI()
    {
        gameplayUI.SetActive(false);
        dialogueUI.SetActive(true);
    }

    public void HideDialogueUI()
    {
        gameplayUI.SetActive(true);
        dialogueUI.SetActive(false);
    }
}
