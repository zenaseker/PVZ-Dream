using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCan : PropBase
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
        this.GetComponent<Animator>().Play("anim_water");
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
            plantBase.ReCoverHp(plantBase.MaxHP - plantBase.HP);
        }
        PropManage.Instance.OnPropUsed();
    }
}
