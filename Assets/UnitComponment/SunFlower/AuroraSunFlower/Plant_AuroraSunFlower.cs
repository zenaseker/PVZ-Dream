using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AuroraAura;

public class Plant_AuroraSunFlower : Plant_SunFlower
{
    List<Vector3> downwave = new List<Vector3>();
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        DefaultProductTime = ProductTime = 15;
        Productingtime = 10;
    }
    public override void Product()
    {
        Invoke("InitEffect", 0.3f);
        Invoke("InitSun", 0.8f);
        ChangeLight(1.3f,0.8f);
        HighLighttime = 1.6f;
    }
     
    public void CheckAuroraNumber()
    {
        int num = MapManage.Instance.meshPlants[XY.x,XY.y].mapbuflist.Find(x=>x is MapBuf_Aurora).stack - 1;
        for (int i = 0; i < 6; i++)
        {
            this.transform.GetChild(0).Find("Pitons").GetChild(i).gameObject.SetActive(i <= num);
        }
    }

    private void InitSun()
    {
        foreach(Vector3 vector in downwave)
        {
            GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "FrostStream", vector);
            InitBullet(obj.GetComponent<FrostStream>());
            RandomUtil.AddOrGetComponent<FrostStream>(obj).Init(1, 1.2f, 1f);
            OnProduct(BattleManage.CreateSun(vector, 15, true));
        }
    }
    private void InitEffect()
    {
        int num = 0;
        if(MapManage.Instance.meshPlants[XY.x, XY.y].mapbuflist.Find(x => x is MapBuf_Aurora) != null)
        {
            num= MapManage.Instance.meshPlants[XY.x, XY.y].mapbuflist.Find(x => x is MapBuf_Aurora).stack / 2;
        }
        if (num <= 0)
        {
            num = 1;
        }
        downwave = new List<Vector3>();
        for(int i = 0; i < num; i++)
        {
            Vector3 vector = new Vector3(Random.Range(MapManage.Instance.meshpos[0, MapManage.Instance.meshxy.y - 5].x, MapManage.Instance.meshpos[0, MapManage.Instance.meshxy.y - 1].x),
                Random.Range(MapManage.Instance.meshpos[0, 0].y, MapManage.Instance.meshpos[MapManage.Instance.meshxy.x - 1, 0].y), 0);
            GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "IceMeteorBullet", vector);
            RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.5f);
            downwave.Add(vector);
        }
    }
}
