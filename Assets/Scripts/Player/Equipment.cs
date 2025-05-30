using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;

    private PlayerController controller;
    [SerializeField] private Equip defaultUnarmed;

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
        if (context.phase == InputActionPhase.Started && curEquip != null )
        { 
            curEquip.OnAttackInput(); // 현재 장비된 맨손 스크립트 실행
        }
    }
}
