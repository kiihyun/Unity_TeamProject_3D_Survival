using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    public override void OnAttackInput()
    {
        if (attacking) return;

        attacking = true;
        Invoke(nameof(OnCanAttack), attackRate);

        PerformAttack();
    }
    void PerformAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, attackDistance))
        {
            if (hit.collider.TryGetComponent(out IDamagable target))
            {
                target.TakePhysicalDamage(damage);
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
