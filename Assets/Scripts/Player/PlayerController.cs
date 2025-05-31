using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour, IMovable, ISprintable, ILookable, IJumpable
{
    [Header("Movement")]
    public float curSpeed;
    public float walkSpeed;         //속도
    public float sprintSpeed;
    public Vector2 curMoveInput;   //이동 입력값
    public bool isRun;

    [Header("Jump")]
    public Transform foot;          //지면 감지
    public LayerMask groundLayer;   //지면 레이어
    public float jumpForce;         //점프 힘
    public AudioClip[] jumpClips;

    [Header("Look")]
    public Transform camContainer;  //카메라 부모오브젝트
    [Range(0.01f, 1f)] public float mouseSensibility;  //마우스감도
    private Vector2 curLookInput;   //시점 입력값
    private float camXRot;          //시점의 x축 회전값

    [Header("Components")]
    public Rigidbody _rigidbody;
    private AudioSource _audioSource;

    [Header("UI 패널 참조")]
    public GameObject inventoryPanel;
    public bool isInventoryOpen = false;

    public bool canControl = true; //플레이어 컨트롤 가능 여부

    public GameObject craftingUI; // 유니티에서 UI 캔버스 연결

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;   //마우스 커서 숨기기 & 잠그기

        //예외처리
        if (!TryGetComponent<Rigidbody>(out _rigidbody))
        {
            Debug.LogError("Rigidbody is null");
        }

        if(!TryGetComponent<AudioSource>(out _audioSource))
        {
            Debug.LogError("AudioSource is null");
        }
    }

    private void Start()
    {
        curSpeed = walkSpeed;

        // 인벤토리를 처음엔 꺼둠
        inventoryPanel.SetActive(false);
        craftingUI.SetActive(false); // 제작 UI도 처음엔 꺼둠
        SetCursorState(false);
    }

    void Update()
    {
        if (!canControl) // UI가 열려 있을 때 ESC로 닫기
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseCraftingUI();
                return;
            }
        }
        if (!canControl) return; //컨트롤 불가능하면 업데이트 중지

        Move();
        if (PlayerManager.Instance.condition.Stamina <= 0)
        {
            curSpeed = walkSpeed; //스태미나가 없으면 이동 불가
        }
    }
    void LateUpdate()
    {
        Look();
    }

    public void SetControl(bool value)
    {
        canControl = value;
        if (value)
        {
            Cursor.lockState = CursorLockMode.Locked; //컨트롤 가능하면 커서 잠금
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; //컨트롤 불가능하면 커서 잠금 해제
        }
    }

    //이동
    public void Move()
    {
        Vector3 moveDir = (transform.forward * curMoveInput.y) + (transform.right * curMoveInput.x);
        UpdateMoveSpeed();
        moveDir *= curSpeed;
        moveDir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = moveDir; //이동 방향으로 속도 설정
    }

    //이동 입력
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (!canControl) return;
        if (context.phase == InputActionPhase.Performed)
        {
            curSpeed = walkSpeed; //걷기 속도로 변경
            curMoveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curSpeed = 0;
            curMoveInput = Vector2.zero;
        }
    }

    //달리기
    public void OnSprintInput(InputAction.CallbackContext context)
    {
        if (!canControl) return;
        if (context.phase == InputActionPhase.Started)
        {
            curSpeed = sprintSpeed; //달리기 속도로 변경
            isRun = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curSpeed = walkSpeed; //걷기 속도로 변경
            isRun = false;
        }
    }

    /// <summary>
    /// 무게에 따라서 이동 속도를 조절하게 만듭니다.
    /// </summary>

    private void UpdateMoveSpeed()
    {
        var weightStatus = PlayerManager.Instance.condition.WeightStatus;

        switch (weightStatus)
        {
            case WeightState.Light:
                curSpeed = walkSpeed * 1.2f;
                break;
            case WeightState.Normal:
                curSpeed = walkSpeed;
                break;
            case WeightState.Heavy:
                curSpeed = walkSpeed * 0.6f;
                break;
            case WeightState.Overloaded:
                curSpeed = 0f;
                break;
        }
    }


    //시점
    public void Look()
    {
        camXRot += curLookInput.y * mouseSensibility;
        camXRot = Mathf.Clamp(camXRot, -80f, 80f);
        camContainer.localEulerAngles = new Vector3(-camXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, curLookInput.x * mouseSensibility, 0);
    }

    //시점 입력
    public void OnLookInput(InputAction.CallbackContext context)
    {
        if (!canControl) return;
        curLookInput = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (!canControl) return;
        if (context.phase == InputActionPhase.Started && IsGrounded() && PlayerManager.Instance.condition.Stamina > PlayerManager.Instance.condition.jumpDecStamina)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            PlayerManager.Instance.condition.JumpStamina(); //점프 시 스태미나 감소
            PlayerManager.Instance.footStep.JumpClipPlay();
            _audioSource.PlayOneShot(jumpClips[Random.Range(0, jumpClips.Length)]);
        }
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

    public void OpenCraftingUI()
    {
        craftingUI.SetActive(true); // UI 띄우기
        SetControl(false);          // 플레이어 조작 막기
        Cursor.visible = true;      // 마우스 커서 보이기
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnOpenCraftingInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OpenCraftingUI();
        }
    }
    public void CloseCraftingUI()
    {
        craftingUI.SetActive(false);      // UI 끄기
        SetControl(true);                 // 플레이어 조작 다시 허용
        Cursor.visible = false;           // 마우스 커서 숨기기
        Cursor.lockState = CursorLockMode.Locked;
    }

    //지면 감지
    public bool IsGrounded()
    {
        Ray[] ray = new Ray[4]
        {
            new Ray(foot.position + (Vector3.forward * 0.2f) + (Vector3.right * 0.2f), Vector3.down),
            new Ray(foot.position - (Vector3.forward * 0.2f) - (Vector3.right * 0.2f), Vector3.down),
            new Ray(foot.position + (Vector3.forward * 0.2f) - (Vector3.right * 0.2f), Vector3.down),
            new Ray(foot.position - (Vector3.forward * 0.2f) + (Vector3.right * 0.2f), Vector3.down),
        };

        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], 0.2f, groundLayer))
            {
                return true;
            }
        }
        return false;
    }
}
