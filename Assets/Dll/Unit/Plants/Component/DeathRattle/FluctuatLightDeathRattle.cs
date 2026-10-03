using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluctuatLightDeathRattle : DeathRattleBase
{
    public override void Destory()
    {
        base.Destory();
        GameObject obj = PoolManage.Instance.GetPoolGameObject("Bullet", "Sun", this._detail._owner.transform.position);
        RandomUtil.AddOrGetComponent<ObjectJump>(obj).Move(obj.transform.position + new Vector3(Random.Range(-1f,1f),-1,0), obj.gameObject.GetComponent<Sun>().CheckSun);
    }
}
