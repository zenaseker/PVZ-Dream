using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitBuf
{
    public BattleUnitModel _owner;
    public int stack = 0;
    public float countdown;
    public int Maxstack = 100;
    public virtual KeyWordBuf KeyWordBuf
    {
        get
        {
            return KeyWordBuf.Default;
        }
    }
    #region 通用

    public void Init(BattleUnitModel owner)
    {
        _owner = owner;
        _owner.bufDetail.OnDie += OnDie;
        _owner.bufDetail.OnUpdate += OnUpdate;
        _owner.bufDetail.OnTakeDamage += OnTakeDamage;
        _owner.bufDetail.OnAttack += OnAttack;
        _owner.bufDetail.OnProduct += OnProduct;
        _owner.bufDetail.OnGetBullet += OnGetBullet;
        this.OnInit();
    }
    public virtual int MaxHp()
    {
        return 0;
    }
    public virtual float StartMaxSpeed()
    {
        return 0f;
    }
    public virtual float StartMinSpeed()
    {
        return 0f;
    }
    public virtual Color UnitColor()
    {
        return Color.white;
    }
    public virtual void OnInit()
    {

    }
    public virtual void AddStack(int stack,float countDown)
    {
        this.stack += stack;
        if (this.stack > Maxstack)
        {
            this.stack = Maxstack;
        }
        this.countdown += countDown;
        OnAdd();
    }
    public virtual void OnAdd()
    {

    }
    public virtual void OnDie()
    {
    }
    public void Destory()
    {
        if (_owner == null) return;
        _owner.bufDetail.OnDie -= OnDie;
        _owner.bufDetail.OnUpdate -= OnUpdate;
        _owner.bufDetail.OnTakeDamage -= OnTakeDamage;
        _owner.bufDetail.OnAttack -= OnAttack;
        _owner.bufDetail.OnProduct -= OnProduct;
        _owner.bufDetail.removelist.Add(this);
    }
    public virtual void OnUpdate(float deltatime)
    {

    }
    public virtual void OnDestory()
    {

    }
    public virtual void OnTakeDamage(int dmg,DamageElement damageType)
    {

    }
    public virtual void OnAttack(DamageObject damageObject)
    {

    }
    public virtual void OnGetBullet(IEnchantment enchantment)
    {

    }
    public virtual float SpeedChange()
    {
        return 1f;
    }
    public virtual float TakeDamageChange(int dmg, DamageElement damageType)
    {
        return 1f;
    }//受到的伤害变动(乘算)
    public virtual int TakeDamageChangeInt(int dmg, DamageElement damageType)
    {
        return 0;
    }//受到的伤害变动(加算)
    public virtual float GiveDamageChange(int dmg, DamageElement damageType)
    {
        return 1f;
    }//造成的伤害变动(乘算)
    #endregion

    #region 植物专用
    public virtual float ProductTime()//生产时间增加百分比
    {
        return 1f;
    }
    public virtual void OnProduct(GameObject sun)//生产时
    {

    }
    public virtual float EatingTime()//咀嚼时间
    {
        return 0;
    }
    #endregion



    #region 僵尸专用


    #endregion
}
