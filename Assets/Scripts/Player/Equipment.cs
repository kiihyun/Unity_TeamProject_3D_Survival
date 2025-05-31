using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Animator animator;

    private PlayerController controller;
    [SerializeField] private Equip defaultUnarmed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        EquipNew(defaultUnarmed);
    }

    public void EquipNew(Equip newEquip)
    {
        curEquip = newEquip;
    }


    public void UnEquip()
    {
        curEquip = null;
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Attack");
        if (context.phase == InputActionPhase.Started && curEquip != null )
        { 
            curEquip.OnAttackInput(); // 현재 장비된 맨손 스크립트 실행
        }
    }
}
