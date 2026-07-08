using System;
using UnityEngine;

public class knight : base_class
{

    public override void Attack(base_class target)
    {
        _ArmorClass = 2;
        target.TakeDamage(AttackPower);
    }

    public override void TakeDamage(int dmgAmount)
    {
        int DamageAfterArmor = Mathf.Max(1, dmgAmount - _ArmorClass);
        base.TakeDamage(DamageAfterArmor);
    }

}