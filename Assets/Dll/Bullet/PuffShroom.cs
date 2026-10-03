using System.Collections.Generic;
using UnityEngine;
using static BlinkArea;

public class PuffShroom : BulletBase
{
    public override void Init(Vector2 flyspeed, int startline, UnitFaction unitFaction, int dmg = -1)
    {
        base.Init(flyspeed, startline, unitFaction, dmg);
        MusicManage.Instance.PlayEffect("puff", 1);
    }
    public override void OnHit(BattleUnitModel unit)
    {
        MusicManage.Instance.PlayEffect(RandomUtil.SelectOne(Attribute.Instance.NormalMusic["PeaBulletHit"]), 1);
        base.OnHit(unit);
    }
    public override void HitEffect()
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "PuffShroomHitPS", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.5f);
    }
    public override void AfterHit()
    {
        PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject);
    }
}
