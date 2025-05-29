using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotkeySlotUI : MonoBehaviour, IDropHandler
{
    //여기는 아이템을 드롭할 대상에 대한 스크립트입니다.
    public int slotIndex; // 0~8 (숫자키 1~9번에 해당)
    public Image iconImage;

    public ShortcutManager shortcutManager;

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<InventorySlotUI>();
        if (dragged == null) return;

        ItemData item = dragged.GetItem();
        if (item == null) return;

        shortcutManager.SetShortcut(slotIndex, item);
        iconImage.sprite = item.icon;
        iconImage.enabled = true;
    }
}
