using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_IceCrystalWallNut : Plant_WallNut
{
    private int totaldmg = 0;
    public override void OnTakeDamage(int damage, BattleUnitModel attacker, DamageElement damageType)
    {
        base.OnTakeDamage(damage, attacker, damageType);
        attacker?.bufDetail.AddKeyWordBuf(KeyWordBuf.Cold,2, 5f);
    }
    public override void OnHpChange(int num)
    {
        base.OnHpChange(num);
        if (num > 0)
        {
            totaldmg += num;
        }
        if (totaldmg > MaxHP / 3)
        {
            GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "FrostStream", this.transform.position);
            RandomUtil.AddOrGetComponent<FrostStream>(obj).Init(4,2.5f, 1f);
            totaldmg = 0;
        }
    }
}