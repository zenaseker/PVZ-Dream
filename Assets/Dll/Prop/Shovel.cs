using System.Collections.Generic;
using UnityEngine;

public class Shovel : PropBase
{
    public override void OnClick()
    {
        base.OnClick();
        if (inplant != null)
        {
            MusicManage.Instance.PlayEffect("plant2", 1);
            inplant.Die();
            PropManage.Instance.OnPropUsed();
            return;
        }
        PropManage.Instance.CancelProp();
    }
    public override void OnHide()
    {
        base.OnHide();
    }
    public override void OnShow()
    {
        base.OnShow();
        MusicManage.Instance.PlayEffect("shovel", 1);
        inplant = null;
    }
}
