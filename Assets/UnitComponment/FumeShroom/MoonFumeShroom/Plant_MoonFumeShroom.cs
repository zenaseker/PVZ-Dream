using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuffManage;

public class Plant_MoonFumeShroom : Plant_FumeShroom
{
    public override void OnAttack()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "BigPuffShroom_Dark", ObjCreateTsf.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(gameObject).Init(2f);
        MusicManage.Instance.PlayEffect("fume", 1);
    }
    public override void FumeShroomHit(ZombiesBase zombie)
    {
        if (zombie.bufDetail.buflist.Find(x=> x is BattleUnitBuf_MoonErosion) != null)
        {
            ((BattleUnitBuf_MoonErosion)zombie.bufDetail.buflist.Find(x => x is BattleUnitBuf_MoonErosion)).Maxstack--;
            zombie.bufDetail.buflist.Find(x => x is BattleUnitBuf_MoonErosion).AddStack(0,0);
        }
    }
}
