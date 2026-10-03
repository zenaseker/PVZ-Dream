using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl_SnowStrom : BattleControlBase
{
    float time = 0f;
    bool instrom = false;
    GameObject effcet;
    public override void OnLevelStart()
    {
        base.OnLevelStart();
        DebugShow.Instance.Init("暴风雪将在60秒后到来！");
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        time += Time.deltaTime;
        if (time >= 60f && !instrom)
        {
            instrom = true;
            DebugShow.Instance.Init("暴风雪来了！");
            if (effcet == null)
            {
                effcet = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "SnowStorm");
            }
            effcet.SetActive(true);
        }
        if (instrom)
        {
            Transform Plants = GameObject.Find("PlantManage").transform;
            for(int i = 0;i < Plants.childCount; i++)
            {
                if (Plants.GetChild(i).TryGetComponent<BattleUnitModel>(out BattleUnitModel model))
                {
                    model.bufDetail.AddKeyWordBuf(KeyWordBuf.Cold, 2, 1);
                }
            }
            Transform zombies = GameObject.Find("ZombieManage").transform;
            for (int i = 0; i < zombies.childCount; i++)
            {
                if (zombies.GetChild(i).TryGetComponent<BattleUnitModel>(out BattleUnitModel model))
                {
                    model.bufDetail.AddKeyWordBuf(KeyWordBuf.Cold, 2, 1);
                }
            }
        }
        if(time >= 120f && instrom)
        {
            DebugShow.Instance.Init("暴风雪暂时停歇了……");
            instrom = false;
            effcet.SetActive(false);
            time = 0f;
        }
    }
}
