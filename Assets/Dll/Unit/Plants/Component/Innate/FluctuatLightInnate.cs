using UnityEngine;

public class FluctuatLightInnate : InnateBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Init(detail);
        int num = BattleManage.Instance.SunNumber;
        if(num > 30)
        {
            num = 30;
        }
        if (num < 0)
        {
            num = 0;
        }
        BattleManage.Instance.SunnumberChange(-num);
        this._detail._owner.BuildTime -= num;
    }
}
