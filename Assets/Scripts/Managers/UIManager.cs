using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("패널 참조")]
    public GameObject gameplayUI;
    public GameObject dialogueUI;
    public GameObject uiInventory;
    public GameObject uiCrafting;

    private PlayerController playerController;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        uiInventory.SetActive(false);
        uiCrafting.SetActive(false);
    }

    public void OpenInventory()
    {
        uiInventory.SetActive(true);
        uiCrafting.SetActive(false);
        // 플레이어 컨트롤러 비활성화
        playerController?.SetControl(false);
    }

    public void OpenCrafting()
    {
        uiCrafting.SetActive(true);
        uiInventory.SetActive(false);
        // 플레이어 컨트롤러 비활성화
        playerController?.SetControl(false);
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
