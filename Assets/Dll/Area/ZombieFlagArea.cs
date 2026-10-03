using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BlinkArea;

public class ZombieFlagArea : AreaBase
{
    public Action remove;
    public override void InArea(BattleUnitModel battleUnitModel)
    {
        if (battleUnitModel is ZombiesBase)
        {
            remove += battleUnitModel.bufDetail.AddBuf(new ZombieBuf_Flag()).Destory;
        }
    }
    public override void OutArea(BattleUnitModel battleUnitModel)
    {
        if (battleUnitModel is ZombiesBase)
        {
            BattleUnitBuf battleUnitBuf = battleUnitModel.bufDetail.GetBufList().Find(x => x is ZombieBuf_Flag);
            if (battleUnitBuf != null)
            {
                remove -= battleUnitBuf.Destory;
                battleUnitModel.bufDetail.RemoveBuf(x => x == battleUnitBuf);
            }
        }
    }
    public void OnDestroy()
    {
        remove?.Invoke();
    }
    public void OnDisable()
    {
        remove?.Invoke();
    }
    public class ZombieBuf_Flag : BattleUnitBuf
    {
        public override float SpeedChange()
        {
            return 1.2f;
        }
        public override void OnAdd()
        {
            base.OnAdd();
            _owner.ChangeSpeed();
        }
        public override void OnDestory()
        {
            _owner.ChangeSpeed();
            base.OnDestory();
        }
    }

}
