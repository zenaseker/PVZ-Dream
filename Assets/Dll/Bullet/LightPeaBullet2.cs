
using UnityEngine;
using static BlinkArea;

public class LightPeaBullet2 : PeaBullet
{
    public override void OnHit(BattleUnitModel unit)
    {
        unit?.bufDetail.AddBuf(new ZombieBuf_BlinkArea(), 1, 5f);
        base.OnHit(unit);
    }
    public override void HitEffect()
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "LightPeaBulletHitPS", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.5f);
    }
}