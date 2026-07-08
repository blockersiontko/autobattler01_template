using System;
using UnityEngine;

public class archer : base_class
{

    public override void Attack(base_class target)
    {
        _AttackPower = UnityEngine.Random.Range(5, 15);
        target.TakeDamage(AttackPower);
    }

    public override void TakeDamage(int dmgAmount)
    {
        base.TakeDamage(dmgAmount);
    }

}