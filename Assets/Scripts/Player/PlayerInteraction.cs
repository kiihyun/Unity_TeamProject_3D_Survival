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
        curDetectObject.GetComponent<ItemObject>().OnInteract();
        Debug.Log(curDetectObject.gameObject.name + "와 상호작용 성공");

        curDetectObject.GetComponent<ItemPickup>().ItemInteract(inventory);
        //Songdo 아이템 오브젝트에 붙어있는 itempickup에 들어있는 ItemInteract 함수를 통해서 플레이어 인벤토리에 정보를 넘겨줌
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
            if (curDetectObject != null) { return; }
            promptUI.text = curDetectObject.GetComponent<ItemObject>().GetInteractPrompt();    // 감지된 오브젝트의 상호작용 프롬프트를 가져옴
        }
        else
        {
            lastHit = default;
            hasHit = false;
            promptUI.text = string.Empty;
        }
    }
}