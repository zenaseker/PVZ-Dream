using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPeaBullet : PeaBullet
{
    public override void Init(Vector2 flyspeed, int startline, UnitFaction unitFaction, int dmg = -1)
    {
        damageType = DamageElement.Snow;
        base.Init(flyspeed, startline, unitFaction, dmg);
    }
    public override void OnHit(BattleUnitModel unit)
    {
        unit.bufDetail.AddKeyWordBuf(KeyWordBuf.Cold, 2, 10f);
        base.OnHit(unit);
    }
    public override void HitEffect()
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "SnowHitPS", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(1f);
    }
}



