using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkDeathRattle : DeathRattleBase
{
    public override void Destory()
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("Area", "BlinkArea", this._detail._owner.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(30f);
        base.Destory();
    }
}
