using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_ChargeFootball : Zombie_Football
{
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed = 1)
    {
        base.Init(order, zombieCard, line, startspeed);
        this.bufDetail.AddBuf(new BattleUnitBuf_ChargeFootball());
    }
    public override void Attack()
    {
        base.Attack();
        if (unitbase != null && Random.Range(0f,1f) <= 0.5f)
        {
            unitbase.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 1);
            this.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 1);
        }
    }
    public void OnMoonBreak()
    {
        if (unitbase != null)
        {
            unitbase = null;
            this.GetComponent<Animator>().SetBool("Attack", false);
        }
    }
    public class BattleUnitBuf_ChargeFootball : BattleUnitBuf
    {
        public override float TakeDamageChange(int dmg, DamageElement damageType)
        {
            if (damageType == DamageElement.Dark)
            {
                return -0.5f;
            }
            return base.TakeDamageChange(dmg, damageType);
        }
    }
}
