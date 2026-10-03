using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCrystalDeathRattle: DeathRattleBase
{
    public override void Destory()
    {
        base.Destory();
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "FrostStream", this._detail._owner.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(1f);
    }
}
