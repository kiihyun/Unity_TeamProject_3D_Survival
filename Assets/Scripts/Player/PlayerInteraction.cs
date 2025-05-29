using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour, IInteractable
{
    [Header("Interact")]
    public GameObject cam;
    public float maxDistance = 3f;
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
        if (itemObject != null)
        {
            //NPC 상호작용 처리
            itemObject.OnInteract();
            Debug.Log(curDetectObject.gameObject.name + "와 상호작용 성공 (NPC)");
            return;
        }

        var npc = curDetectObject.GetComponent<NPCInteraction>();
        if (npc != null)
        {
            //NPC 상호작용 처리
            npc.StartDialogue();
            Debug.Log(curDetectObject.gameObject.name + "와 상호작용 성공 (NPC)");
            return;
        }

        var itemPickup = curDetectObject.GetComponent<ItemPickup>();
        if (itemPickup != null)
        {
            //Songdo 아이템 오브젝트에 붙어있는 itempickup에 들어있는 ItemInteract 함수를 통해서 플레이어 인벤토리에 정보를 넘겨줌
            itemPickup.ItemInteract(inventory);
            Debug.Log(curDetectObject.gameObject.name + "와 상호작용 성공 (Item)");
            return;
        }

        Debug.LogWarning("상호작용 가능한 컴포넌트가 없습니다: " + curDetectObject.name);

    }

    // 오브젝트를 Raycast로 감지하는 메소드
    public void DetectObject()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.Log(hit.collider.gameObject.name + " 감지");
            lastHit = hit;
            hasHit = true;
            curDetectObject = hit.collider.gameObject;

            // 상호작용 프롬프트 표시
            var npc = curDetectObject.GetComponent<NPCInteraction>();
            var item = curDetectObject.GetComponent<ItemObject>();

            if (npc != null)
                promptUI.text = "NPC와 대화하기";
            else if (item != null)
                promptUI.text = item.GetInteractPrompt();
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