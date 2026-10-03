using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl_Night1 : BattleControlBase
{
    public override void OnLevelStart()
    {
        base.OnLevelStart();
        DebugShow.Instance.Init("小型植物可以种植在每格的右下角！");
    }
}
