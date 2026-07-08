using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;

public abstract class base_class : MonoBehaviour
{
    public int _CurrentHP = 100;
    protected int _AttackPower = 10;
    protected int _ArmorClass = 0;

    public int CurrentHP
    {
        get
        {
            return _CurrentHP;
        }
        protected set
        {
            _CurrentHP = Mathf.Max(0, value);
        }
    }

    public bool isAlive => CurrentHP > 0;

    public int AttackPower => _AttackPower;

    public int ArmorClass => _ArmorClass;

    public virtual void TakeDamage(int dmgAmount)
    {
        CurrentHP -= dmgAmount;
        if (_CurrentHP <= 0)
        {
            _CurrentHP = 0;
            Destroy(gameObject);
        }
    }

    public abstract void Attack(base_class target);
}