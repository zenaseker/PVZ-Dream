using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDetail
{
    [HideInInspector]public PlantBase _owner;
    public List<AuraBase> _aura = new List<AuraBase>();//光环
    public List<DeathRattleBase> _deathrattle = new List<DeathRattleBase>();//亡语
    public List<EnchantmentBase> _enchantment = new List<EnchantmentBase>();//附魔
    public List<InnateBase> _innate = new List<InnateBase>();//先天
    public MatrixBase _matrix = null;//术阵 
    public void Init(PlantBase battleUnitModel)
    {
        _owner = battleUnitModel;
        foreach(AuraBase aura in _aura)
        {
            aura.Init(this);
        }
        foreach (DeathRattleBase deathrattle in _deathrattle)
        {
            deathrattle.Init(this);
        }
        foreach (EnchantmentBase enchantment in _enchantment)
        {
            enchantment.Init(this);
        }
        foreach (InnateBase innate in _innate)
        {
            innate.Init(this);
        }
        _matrix?.Init(this);
    }
    public void Destory()
    {
        foreach (AuraBase aura in _aura)
        {
            aura.Destory();
        }
        foreach (DeathRattleBase deathrattle in _deathrattle)
        {
            deathrattle.Destory();
        }
        foreach (EnchantmentBase enchantment in _enchantment)
        {
            enchantment.Destory();
        }
        foreach (InnateBase innate in _innate)
        {
            innate.Destory();
        }
        _matrix?.Destory();
    }
    public void Attack(IEnchantment enchantment)
    {
        if (_enchantment == null || enchantment == null) return;
        foreach (EnchantmentBase e in _enchantment)
        {
            e.Attack(enchantment);
        }
    }
}
