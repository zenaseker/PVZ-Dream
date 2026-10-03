
using UnityEngine;
using static BlinkArea;

public class NightPeaBullet : PeaBullet
{
    public override void OnHit(BattleUnitModel unit)
    {
        unit?.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 1);
        base.OnHit(unit);
    }
    public override void HitEffect()
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "NightPeaBulletHitPS", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.5f);
    }
}