using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkArea : AreaBase
{
    public override void Init(int line)
    {
        base.Init(line);
        foreach (var monster in Physics2D.OverlapCircleAll(transform.position, 2.5f, 2))
        {
            if (monster.gameObject.tag == "Zombie")
            {
                InArea(monster.gameObject.GetComponent<BattleUnitModel>());
            }
        }
    }
    public override void InArea(BattleUnitModel battleUnitModel)
    {
        if (battleUnitModel is ZombiesBase)
        {
            battleUnitModel.bufDetail.AddBuf(new ZombieBuf_BlinkArea(),0,5);
        }
    }
    public override void OutArea(BattleUnitModel battleUnitModel)
    {
        if (battleUnitModel is ZombiesBase)
        {
            BattleUnitBuf battleUnitBuf = battleUnitModel.bufDetail.GetBufList().Find(x => x is ZombieBuf_BlinkArea);
            if (battleUnitBuf != null)
            {
                battleUnitBuf.Destory();
            }
        }
    }
    public class ZombieBuf_BlinkArea : BattleUnitBuf
    {
        public override void OnUpdate(float deltatime)
        {
            countdown -= deltatime;
            if (countdown <= 0)
            {
                this.Destory();
            }
        }
        public override float GiveDamageChange(int dmg, DamageElement damageType)
        {
            return 0.75f;
        }
        public override float SpeedChange()
        {
            return 0.75f;
        }
        public override void OnAdd()
        {
            base.OnAdd();
            _owner.ChangeSpeed();
        }
        public override void OnDestory()
        {
            base.OnDestory();
            _owner.ChangeSpeed();
        }
    }
}
