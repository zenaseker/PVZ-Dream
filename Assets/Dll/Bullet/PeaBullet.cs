
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : BulletBase
{
    public override void Init(Vector2 flyspeed, int startline, UnitFaction unitFaction, int dmg = -1)
    {
        base.Init(flyspeed, startline, unitFaction, dmg);
        this.transform.Find("shadow").gameObject.SetActive(true);
    }
    public override void OnHit(BattleUnitModel unit)
    {
        if (unit is ZombiesBase)
        {
            if ((unit as ZombiesBase).armor1?.type == ArmorType.Iron || (unit as ZombiesBase).armor2?.type == ArmorType.Iron || (unit as ZombiesBase).armor3?.type == ArmorType.Iron)
            {
                MusicManage.Instance.PlayEffect(RandomUtil.SelectOne(Attribute.Instance.NormalMusic["IconAmrorHited"]), 1);
            }
            else
            {
                MusicManage.Instance.PlayEffect(RandomUtil.SelectOne(Attribute.Instance.NormalMusic["PeaBulletHit"]), 1);
            }
        }
        this.transform.Find("shadow").gameObject.SetActive(false);
        base.OnHit(unit);
    }
    public override void HitEffect()
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "PeaBulletHitPS", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.5f);
    }
    public override void AfterHit()
    {
        PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject);
    }
}
