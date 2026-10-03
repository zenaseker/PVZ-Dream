using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_SnowFumeShroom : Plant_FumeShroom
{
    public override void OnAttack()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "SnowBigPuffShroom", ObjCreateTsf.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(gameObject).Init(2f);
        MusicManage.Instance.PlayEffect("fume", 0.5f);
    }
}
