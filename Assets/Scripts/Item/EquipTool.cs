using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public Animator animator;
    public float attackRate;
    private bool attacking = false;
    public float attackDistance;
    public float useStamina;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void OnAttackInput()
    {
        if (attacking) return;

        attacking = true;
        animator.SetTrigger("Attack");
        Invoke(nameof(OnCanAttack), attackRate);

        PerformAttack();
    }
    void PerformAttack()
    {
        EquipSlot[] equipSlots = PlayerManager.Instance.player.GetComponent<EquipmentSystem>().equipSlots;
        foreach (var slot in equipSlots)
        {
            if (slot.slotType == EquipSlotType.Weapon && slot.equippedItem == null) return;
        }

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, attackDistance))
        {
            if (hit.collider.TryGetComponent(out IDamagable target))
            {
                target.TakePhysicalDamage(damage);
                Debug.Log(animator); // null 여부
            }
        }
    }
    void OnCanAttack()
    {
        attacking = false;
    }
    void OnHit()
    {

    }
}
