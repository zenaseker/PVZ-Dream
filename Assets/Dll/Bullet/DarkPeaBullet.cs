using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPeaBullet : PeaBullet
{
    public override void OnHit(BattleUnitModel unit)
    {
        if (unit?.bufDetail?.GetKeyWordBuf(KeyWordBuf.MoonErosion)?.stack > 0)
        {
            this.dmg += 10;
        }
        base.OnHit(unit);
    }
    public override void HitEffect()
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "NightPeaBulletHitPS", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.5f);
    }
}
