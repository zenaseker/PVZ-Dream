using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostStream : ParitcleSystemBase
{
    public int frostlevel = 2;
    public void Init(int level,float area, float time = 10f)
    {
        MusicManage.Instance.PlayEffect("FrostStream", 0.5f);
        frostlevel = level;
        base.Init(area, area, time);
    }
    public override void Hits(float area)
    {
        base.Hits(area);
        foreach (var monster in Physics2D.OverlapCircleAll(transform.position, area))
        {
            if (monster.gameObject.tag == "Zombie" && !monster.GetComponent<ZombiesBase>().Reverse || monster.gameObject.tag == "Plant")
            {
                Hit(monster.GetComponent<BattleUnitModel>());
            }
        }
    }
    public override void Hit(BattleUnitModel model)
    {
        base.Hit(model);
        if (model is ZombiesBase) model.bufDetail.AddKeyWordBuf(KeyWordBuf.Cold, frostlevel, 20f);
    }
}
