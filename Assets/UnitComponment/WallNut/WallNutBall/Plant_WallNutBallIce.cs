using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Plant_WallNutBallIce : Plant_WallNutBall
{

    public override void OnZombieContact(ZombiesBase zombiesBase)
    {
        base.OnZombieContact(zombiesBase);
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "FrostStream", this.transform.position);
        RandomUtil.AddOrGetComponent<FrostStream>(obj).Init(1, 2.5f, 10f);
    }
}
