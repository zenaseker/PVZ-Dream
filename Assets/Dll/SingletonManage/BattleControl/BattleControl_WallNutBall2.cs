using System.Collections.Generic;
using UnityEngine;

public class BattleControl_WallNutBall2 : BattleControlBase
{
    public override void OnLevelStart()
    {
        ConveyerManage.Instance.plant = new List<int> { 99,599,199,299 };
        ConveyerManage.Instance.plantweight = new int[4];
        ConveyerManage.Instance.plantweight[0] = 8;
        ConveyerManage.Instance.plantweight[1] = 2;
        ConveyerManage.Instance.plantweight[2] = 4;
        ConveyerManage.Instance.plantweight[3] = 4;
    }
}