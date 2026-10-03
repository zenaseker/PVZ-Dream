using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnchantmentBase : ComponentBase
{
    [HideInInspector] public ComponentDetail _detail;
    public virtual void Init(ComponentDetail detail)
    {
        _detail = detail;
    }
    public void Attack(IEnchantment enchantment)
    {
        enchantment.Action += Enchantemnt;
    }
    public virtual void Enchantemnt(BattleUnitModel battleUnitModel ,int dmg)
    {

    }
    public virtual void Destory()
    {
    }
}

//½Ó¿Ú
public interface IEnchantment
{
    public Action<BattleUnitModel,int> Action { get; set; }
}