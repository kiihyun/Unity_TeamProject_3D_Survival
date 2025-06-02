using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// 인벤토리 슬롯 UI이랑 아이템 정보 표시 및 드래그앤드랍을 위한 스크립트입니다.

    private ItemData item;

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI weightText;
    private Outline outline;

    public int index;
    public bool equipped;
    public int quantity;


    //드래그 할 수 있도록 필요한 요소
    private Transform originalParent;
    private Canvas canvas;
    private RectTransform rectTransform;
    //

    private void Awake()
    {
        outline = GetComponent<Outline>();
        //
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        //

        //button.onClick.AddListener(OnClick);

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClickButton);//버튼 클릭 이벤트를 스크립트에서 연결해주는 방식으로 해줘야함!
        }
    }
    // 슬롯에 아이템 세팅
    public void SetItem(ItemData itemData, int quantity = 1)
    {
        item = itemData;

        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
        weightText.text = $"{itemData.itemMass.ToString()} kg";
        
        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }
    // 슬롯을 비우기
    public void ClearItem()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
        weightText.text = string.Empty;
    }
    // 슬롯 클릭하면 아이템 정보 UI 나오도록
    public void OnClickButton()
    {
        //FindObjectOfType<InventoryUI>().OnItemClicked(item);

        //InventoryUI ui = FindObjectOfType<InventoryUI>();
        //if (item != null && ui != null)
        //{
        //    ui.OnItemClicked(item);
        //}

        if (item == null)
        {
            Debug.LogWarning("클릭된 슬롯의 아이템이 null입니다.");
            return;
        }

        var ui = FindObjectOfType<InventoryUI>();
        if (ui != null)
        {
            ui.OnItemClicked(item);
        }
        else
        {
            Debug.LogError("InventoryUI를 찾을 수 없습니다.");
        }
    }

    //드래그 할 경우 그 슬롯을 캔버스 위로 올림
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        originalParent = transform.parent;
        transform.SetParent(canvas.transform); // UI 최상단으로 이동
        icon.raycastTarget = false;

        DragIconManager.Instance.ShowGhost(item.icon);
    }


    /// 드래그하면 마우스를 따라 이동
    public void OnDrag(PointerEventData eventData)
    {
        //이건 고스트 아이콘이 따라다닐 거라서 생략
        //if (item == null) return;

        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }


    /// 드래그 종료 시 제자리록 가고, 아이콘 raycast 활성화 복원
    public void OnEndDrag(PointerEventData eventData)
    {
        if (item == null) return;

        transform.SetParent(originalParent);
        icon.raycastTarget = true;
        rectTransform.anchoredPosition = Vector2.zero;

        DragIconManager.Instance.HideGhost();
    }

    /// 드래그 중인 아이템 데이터를 외부에서 참조할 수 있게 우선 만들어 놓음
    public ItemData GetItem()
    {
        return item;
    }
}
