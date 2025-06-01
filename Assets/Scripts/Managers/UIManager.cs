using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("패널 참조")]
    public GameObject gameplayUI;
    public GameObject dialogueUI;
    public GameObject inventoryPanel;
    public GameObject craftingPanel;
    public GameObject pausePanel;
    public GameObject optionPanel; // 옵션 패널 참조

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
        playerController = FindObjectOfType<PlayerController>();

        inventoryPanel.SetActive(false);
        craftingPanel.SetActive(false);
    }

    public void OnEscPressed()
    {
        if (isInventoryOpen) // 인벤토리 열려 있으면 닫기
        {
            CloseInventory();
            return;
        }
        else if (isCraftingOpen) // 크래프팅 열려 있으면 닫기
        {
            CloseCrafting();
            return;
        }
        else if (isPaused) // 게임이 일시정지 상태면 해제
        {
            ResumeGame();
            return;
        }
        else
        {
            PauseGame();
        }
    }
    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (isInventoryOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    public void OpenInventory()
    {
        // 플레이어 컨트롤러 비활성화
        playerController?.SetControl(false);

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

        // 플레이어 컨트롤러 활성화
        playerController?.SetControl(true);
    }

    public void OnCraftingInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (isCraftingOpen)
            {
                CloseCrafting();
            }
            else
            {
                OpenCrafting();
            }
        }
    }

    public void OpenCrafting()
    {
        // 플레이어 컨트롤러 비활성화
        playerController?.SetControl(false);

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

        // 플레이어 컨트롤러 활성화
        playerController?.SetControl(true);
    }

    public void PauseGame()
    {
        // 플레이어 컨트롤러 비활성화
        playerController?.SetControl(false);

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

        // 플레이어 컨트롤러 활성화
        playerController?.SetControl(true);
    }

    public void ShowDialogueUI()
    {
        // 플레이어 컨트롤러 비활성화
        playerController?.SetControl(false);

        gameplayUI.SetActive(false);
        dialogueUI.SetActive(true);
    }

    public void HideDialogueUI()
    {
        gameplayUI.SetActive(true);
        dialogueUI.SetActive(false);

        // 플레이어 컨트롤러 활성화
        playerController?.SetControl(true);
    }

    public void OptionConfirm()
    {
        optionPanel.SetActive(false); // 옵션 패널 비활성화
        ResumeGame(); // 게임 재개
    }
}
