using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("패널 참조")]
    public GameObject gameplayUI;
    public GameObject dialogueUI;
    public GameObject inventoryPanel;
    public GameObject craftingPanel;
    public GameObject pausePanel;

    private bool isPaused = false;
    private bool isInventoryOpen = false;
    private bool isCraftingOpen = false;

    private PlayerController playerController;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        inventoryPanel.SetActive(false);
        craftingPanel.SetActive(false);
    }

    public void OnEscPressed()
    {
        // 인벤토리 열려 있으면 닫기
        if (isInventoryOpen)
        {
            CloseInventory();
            return;
        }

        // 크래프팅 열려 있으면 닫기
        if (isCraftingOpen)
        {
            CloseCrafting();
            return;
        }

        // Pause 토글
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        craftingPanel.SetActive(false);

        isInventoryOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);

        isInventoryOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void OpenCrafting()
    {
        craftingPanel.SetActive(true);
        inventoryPanel.SetActive(false);

        isCraftingOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void CloseCrafting()
    {
        craftingPanel.SetActive(false);

        isCraftingOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
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
