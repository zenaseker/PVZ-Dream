using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_WallNutBallLight : Plant_WallNutBall
{
    bool hit = false;

    public override void OnZombieContact(ZombiesBase zombiesBase)
    {
        base.OnZombieContact(zombiesBase);
        if (hit) return;
        hit = true;
        GameObject obj = PoolManage.Instance.GetPoolGameObject("Area", "BlinkArea", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(30f);
    }
}
