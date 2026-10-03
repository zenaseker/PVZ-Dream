using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Zombie_Mozzie : ZombiesBase
{
    public override void Attack()
    {
        if (this.losehead) return;
        if (unitbase == null)
        {
            this.GetComponent<Animator>().SetBool("Attack", false);
            return;
        };
        DamageObject damageObject = new DamageObject(this.Damage(0, DamageElement.Default), Bullettype.Zombie, this);
        bufDetail.OnAttack?.Invoke(damageObject);
        unitbase.TakeDamage(damageObject);
        this.ReCoverHp(1);
    }
    public override void Kill(BattleUnitModel target)
    {
        base.Kill(target);
        GameObject obj = ZombieManage.Instance.InitZombie(117, this.Line);
        ZombieManage.Instance.LoadZombieMess(obj, 117, this.Line);
        obj.transform.position = this.transform.position + Vector3.right;
    }
}
