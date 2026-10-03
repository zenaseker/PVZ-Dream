using System.Collections.Generic;

public class BattleControl_WallNutBall : BattleControlBase
{
    public override void OnLevelStart()
    {
        ConveyerManage.Instance.plant = new List<int> { 99,599 };
        ConveyerManage.Instance.plantweight = new int[2];
        ConveyerManage.Instance.plantweight[0] = 10;
        ConveyerManage.Instance.plantweight[1] = 1;
    }
}