using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Cinemachine;

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
    public bool isJump;

    [Header("Look")]
    public Transform camContainer;  //카메라 부모오브젝트
    [Range(0.01f, 1f)] public float mouseSensibility;  //마우스감도
    private Vector2 curLookInput;   //시점 입력값
    private float camXRot;          //시점의 x축 회전값

    [Header("Components")]
    public Rigidbody _rigidbody;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;   //마우스 커서 숨기기 & 잠그기

        //예외처리
        if (!TryGetComponent<Rigidbody>(out _rigidbody))
        {
            Debug.LogError("Rigidbody is null");
        }
    }

    private void Start()
    {
        curSpeed = walkSpeed;
    }

    void Update()
    {
        Move();
    }
    void LateUpdate()
    {
        Look();
    }

    //이동
    public void Move()
    {
        Vector3 moveDir = (transform.forward * curMoveInput.y) + (transform.right * curMoveInput.x);
        moveDir *= curSpeed;
        moveDir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = moveDir;
    }

    //이동 입력
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMoveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMoveInput = Vector2.zero;
        }
    }

    //달리기
    public void OnSprintInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isRun = true;
            curSpeed = sprintSpeed;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRun = false;
            curSpeed = walkSpeed;
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
        curLookInput = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //PlayerManager.Instance.footStep.JumpClipPlay();
        }
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
