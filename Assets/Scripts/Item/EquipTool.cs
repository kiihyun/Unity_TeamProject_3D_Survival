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
        
    }

    void OnCanAttack()
    {

    }
    void OnHit()
    {

    }
}
