using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerInteraction : MonoBehaviour, IInteractable
{
    [Header("Interact")]
    public GameObject cam;
    public float maxDistance = 3f;
    public Collider detectedCollider; //감지된 오브젝트를 저장할 변수


    void Start()
    {

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
        Debug.Log(detectedCollider.gameObject.name + "와 상호작용 성공");
    }

    // 오브젝트를 Raycast로 감지하는 메소드
    public Collider DetectObject()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.Log(hit.collider.gameObject.name + " 감지");
            detectedCollider = hit.collider; //감지된 오브젝트를 저장
        }

        return detectedCollider;
        //감지한 오브젝트를 반환하는 방식으로 하면 좋을 듯
    }
}