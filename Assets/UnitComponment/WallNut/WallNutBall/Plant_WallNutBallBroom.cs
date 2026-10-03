using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_WallNutBallBroom : Plant_WallNutBall
{

    public override void OnZombieContact(ZombiesBase zombiesBase)
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "CherryBroomPS", this.transform.position);
        RandomUtil.AddOrGetComponent<CherryBroom>(obj).Init(1800, true, 2.2f, 0.5f);
        this.Destory();
    }
}
