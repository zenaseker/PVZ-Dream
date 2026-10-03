using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_JooHeroFumeShroom : Plant_FumeShroom
{
    int count = 3;
    public class BattleUnitBuf_JooHeroPossess : BuffManage.BattleUnitBuf_Possess
    {
        public override PossessType possessType
        {
            get
            {
                return PossessType.FumeShroom;
            }
        }
        public override void OnInit()
        {
            base.OnInit();
        }
        public override void OnUpdate(float deltatime)
        {
            base.OnUpdate(deltatime);
        }
        public override int BrokenNum => 1;
        public override void OnDie()
        {
            base.OnDie();
            BattleManage.Instance.CreateJumpCard(408, this._owner.transform.position);
        }
    }
    public override void FumeShroomHit(ZombiesBase zombie)
    {
        if(count > 0 && UnityEngine.Random.Range(0f,1f) >= 0.5f)
        {
            if (zombie.bufDetail.GetBufList().Find(x => x is BattleUnitBuf_JooHeroPossess) != null)
            {
                return;
            }
            if (zombie.bufDetail.GetKeyWordBuf(KeyWordBuf.Possess) != null)
            {
                zombie.bufDetail.RemoveBuf(x => x.KeyWordBuf == KeyWordBuf.Possess);
            }
            zombie.bufDetail.AddBuf(new BattleUnitBuf_JooHeroPossess());
            count--;
        }
    }
}
