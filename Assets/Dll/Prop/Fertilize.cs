using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilize : PropBase
{
    private bool inhammer = false;
    public override void Update()
    {
        if (inhammer) return;
        base.Update();
    }
    public override void OnClick()
    {
        if (inplant == null || MapManage.Instance.meshPlants[inplant.XY.x, inplant.XY.y].GetPlants().Count <= 0)
        {
            PropManage.Instance.CancelProp();
            return;
        }
        inhammer = true;
        this.GetComponent<Animator>().Play("Fertilize");
        base.OnClick();
    }
    public override void OnHide()
    {
        base.OnHide();
        inhammer = false;
    }
    private void AfterAnimator()
    {
        if (inplant == null)
        {
            return;
        }
        else if (MapManage.Instance.meshPlants[inplant.XY.x, inplant.XY.y].GetPlants().Count <= 0)
        {
            inplant.HighLighttime = 0;
            inplant.ChangeLight(1f);
            PropManage.Instance.CancelProp(); 
            return;
        }
        foreach (PlantBase plantBase in MapManage.Instance.meshPlants[inplant.XY.x, inplant.XY.y].GetPlants())
        {
            if (plantBase.BuildTime > 0)
            {
                plantBase.BuildTime = 0.5f; 
            }
            MusicManage.Instance.PlayEffect("fertilizer", 1);
            GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "BulidingPS", plantBase.transform.position);
            RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.5f);
        }
        PropManage.Instance.OnPropUsed();
    }
}
