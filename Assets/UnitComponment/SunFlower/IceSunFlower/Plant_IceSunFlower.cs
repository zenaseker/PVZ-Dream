using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_IceSunFlower : Plant_SunFlower
{
    Vector3 downwave;
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        DefaultProductTime = ProductTime = 15;
        Productingtime = 5;
    }
    public override void Product()
    {
        Invoke("InitEffect", 0.3f);
        Invoke("InitSun", 0.8f);
        ChangeLight(1.3f,0.8f);
        HighLighttime = 1.6f;
    }

    private void InitSun()
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "FrostStream", downwave);
        InitBullet(obj.GetComponent<FrostStream>());
        RandomUtil.AddOrGetComponent<FrostStream>(obj).Init(1,2f, 1f);
        OnProduct(BattleManage.CreateSun(downwave, 15, true));
    }
    private void InitEffect()
    {
        downwave = new Vector3(Random.Range(MapManage.Instance.meshpos[XY.x, XY.y].x, MapManage.Instance.meshpos[XY.x, MapManage.Instance.meshxy.y - 1].x), MapManage.Instance.meshpos[XY.x, XY.y].y, 0);
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "IceMeteorBullet", downwave);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.5f);
    }
}
