using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Animator animator;
    public SkinnedMeshRenderer meshRenderer;
    public Equip defaultUnarmed;
    private bool isAttacking;

    private PlayerController controller;
    private ItemData curWeapon;


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
        EquipSlot[] equipSlots = PlayerManager.Instance.player.GetComponent<EquipmentSystem>().equipSlots;
        Debug.Log($"장착:{equipSlots.ToString()}");
        foreach (var slot in equipSlots) 
        {
            Debug.Log(slot);
            if (slot.slotType == EquipSlotType.Weapon && slot.equippedItem != null) return;
        }

        if (context.phase == InputActionPhase.Started && curEquip != null && !EventSystem.current.IsPointerOverGameObject())
        {
            meshRenderer.enabled = true;
            animator.SetTrigger("Attack");
            curEquip.OnAttackInput(); // 현재 장비된 맨손 스크립트 실행
        }
    }

    void OnAttackEnd()
    {
        meshRenderer.enabled = false;
        isAttacking = false;
    }
    public void OnAttackAnimationEnd()
    {
        meshRenderer.enabled = false;
        isAttacking = false;

    }
}
