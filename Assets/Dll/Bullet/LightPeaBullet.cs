using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPeaBullet : PeaBullet
{
    public override void Init(Vector2 flyspeed, int startline, UnitFaction unitFaction, int dmg = -1)
    {
        base.Init(flyspeed, startline, unitFaction, dmg);
        this.dmg = (dmg == -1)?20: dmg;
        int dmgup = BattleManage.Instance.SunNumber / 5;
        if (dmgup > 30)
        {
            dmgup = 30;
        }
        if (dmgup < 0)
        {
            dmgup = 0;
        }
        this.dmg = 20 + dmgup;
    }
    public override void HitEffect()
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "LightPeaBulletHitPS", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.5f);
    }
}
