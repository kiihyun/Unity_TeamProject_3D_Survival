using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour, IInteractable
{
    [Header("Interact")]
    public GameObject cam;
    public float maxDistance = 3f;
    public int hitTreeCount;
    public int maxHitTreeCount;
    public RaycastHit lastHit { get; private set; }
    public bool hasHit { get; private set; }
    public GameObject curDetectObject; //감지된 오브젝트를 저장할 변수
    public TextMeshProUGUI promptUI;


    private Inventory inventory;//Songdo 플레이어가 가지는 인벤토리

    void Start()
    {
        inventory = GetComponent<Inventory>();//Songdo 인벤토리 가져옴
    }

    void Update()
    {
        DetectObject();
    }

    // InputAction을 통해 상호작용 입력을 받는 메소드
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Interact(); //상호작용 메소드 호출
        }
        //아이템 데이터 가져오기
    }

    // 감지된 오브잭트를 이용하여 상호작용하는 메소드
    public void Interact()
    {
        if (curDetectObject == null) return;

        var itemObject = curDetectObject.GetComponent<ItemObject>();
        var stone = curDetectObject.GetComponent<Stone>();
        if (itemObject != null && stone == null)
        {
            //itemObject 상호작용 처리
            itemObject.OnInteract();
#if UNITY_EDITOR
            Debug.Log(curDetectObject.gameObject.name + "와 상호작용 성공 (itemObject)");
#endif
            return;
        }

        var npc = curDetectObject.GetComponent<NPCInteraction>();
        if (npc != null)
        {
            //NPC 상호작용 처리
            npc.StartDialogue();
#if UNITY_EDITOR
            Debug.Log(curDetectObject.gameObject.name + "와 상호작용 성공 (NPC)");
#endif
            return;
        }

        var itemPickup = curDetectObject.GetComponent<ItemPickup>();
        if (itemPickup != null)
        {
            //Songdo 아이템 오브젝트에 붙어있는 itempickup에 들어있는 ItemInteract 함수를 통해서 플레이어 인벤토리에 정보를 넘겨줌
            itemPickup.ItemInteract(inventory);
#if UNITY_EDITOR
            Debug.Log(curDetectObject.gameObject.name + "와 상호작용 성공 (Item)");
#endif
            return;
        }

        
        if (stone != null)
        {
            stone.ItemInteract(inventory);
#if UNITY_EDITOR
            Debug.Log(curDetectObject.gameObject.name + "와 상호작용 성공 (Item)");
#endif
            return;
        }



        Debug.LogWarning("상호작용 가능한 컴포넌트가 없습니다: " + curDetectObject.name);

    }

    public void Attack(InputAction.CallbackContext context)
    {
        // 기본 null 방지 체크
        if (PlayerManager.Instance == null ||
            PlayerManager.Instance.controller == null ||
            PlayerManager.Instance.player == null)
        {
            Debug.LogWarning("PlayerManager의 필드가 null입니다. 연결을 확인하세요.");
            return;
        }

        if (!PlayerManager.Instance.controller.isInventoryOpen && context.phase == InputActionPhase.Started)
        {
            // EquipmentSystem 컴포넌트 존재 여부 확인
            var equipmentSystem = PlayerManager.Instance.player.GetComponent<EquipmentSystem>();
            if (equipmentSystem == null)
            {
                Debug.LogWarning("Player에 EquipmentSystem 컴포넌트가 없습니다.");
                return;
            }

            EquipSlot[] slots = equipmentSystem.equipSlots;

            foreach (var slot in slots)
            {
                if (slot == null || slot.equippedItem == null) continue;

                if (slot.slotType == EquipSlotType.Weapon &&
                    slot.equippedItem.displayName == "플레어건") // 이름이 정확히 일치해야 함
                {
                    Debug.Log("플레어건으로 엔딩 진입!");
                    SceneManager.LoadScene("EndingScene");
                    return;
                }
            }

            // 나무를 공격하는 로직
            if (curDetectObject != null)
            {
                var tree = curDetectObject.GetComponent<Tree>();
                if (tree != null)
                {
                    hitTreeCount++;
                    if (hitTreeCount == maxHitTreeCount)
                    {
                        tree.ItemInteract(inventory);
                        Debug.Log($"{curDetectObject.name}와 상호작용 성공 (나무)");
                        hitTreeCount = 0;
                        return;
                    }
                }
            }
        }
    }


    // 오브젝트를 Raycast로 감지하는 메소드
    public void DetectObject()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            lastHit = hit;
            hasHit = true;
            curDetectObject = hit.collider.gameObject;

            // 상호작용 프롬프트 표시
            var npc = curDetectObject.GetComponent<NPCInteraction>();
            var item = curDetectObject.GetComponent<ItemObject>();
            var tree = curDetectObject.GetComponent<Tree>();

            if (npc != null)
                promptUI.text = "NPC와 대화하기";
            else if (item != null)
                promptUI.text = item.GetInteractPrompt();
            else if (tree != null)
                promptUI.text = "나무";
            else
                promptUI.text = string.Empty;
        }
        else
        {
            lastHit = default;
            hasHit = false;
            curDetectObject = null;
            promptUI.text = string.Empty;
        }
    }
}