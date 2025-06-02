using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIManager : MonoBehaviour
{
    /// 인벤토리 열고 닫기 위해서 있는 스크립트입니다.

    [Header("UI 패널 참조")]
    public GameObject inventoryPanel;

    [Header("입력")]
    public PlayerInput playerInput; // PlayerInput 컴포넌트

    private bool isInventoryOpen = false;
    void Start()
    {
        // 인벤토리를 처음엔 꺼둠
        inventoryPanel.SetActive(false);
        SetCursorState(false);
    }

    // Input System의 "Inventory" 액션에 연결됨
    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
        SetCursorState(isInventoryOpen);
    }

    private void SetCursorState(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
