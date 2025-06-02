using UnityEngine;

public class ShortcutManager : MonoBehaviour
{
    public const int ShortcutCount = 9;
    private ItemData[] shortcuts = new ItemData[ShortcutCount];


    //숏컷에 아이템 등록
    public void SetShortcut(int index, ItemData item)
    {
        if (index < 0 || index >= ShortcutCount) return;
        shortcuts[index] = item;
    }
    //숏컷에 접근
    public ItemData GetShortcut(int index)
    {
        if (index < 0 || index >= ShortcutCount) return null;
        return shortcuts[index];
    }
}
