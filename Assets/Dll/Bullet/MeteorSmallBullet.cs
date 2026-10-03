using UnityEngine;

public class MeteorSmallBullet : BulletBase
{
    public override void OnHit(BattleUnitModel unit)
    {
        base.OnHit(unit);
    }
    public override void HitEffect()
    {
        base.HitEffect();
        MusicManage.Instance.PlayEffect("firepea", 1);
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "FirePS", transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.22f);
    }
    public override void AfterHit()
    {
        PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject);
    }
}
