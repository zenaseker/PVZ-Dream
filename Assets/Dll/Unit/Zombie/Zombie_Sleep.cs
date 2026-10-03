using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Sleep : Zombie_Common
{
    private bool sleep = true;
    public GameObject face;
    public override void TakeDamage(DamageObject damageObject)
    {
        base.TakeDamage(damageObject);
        if (sleep && damageObject.Damage >=40)
        {
            sleep = false;
            face.gameObject.SetActive(false);
            this.unitInfo.Damage += 10;
            this.bufDetail.AddBuf(new BattleUnitBuf_wakeup());
        }
    }
    public class BattleUnitBuf_wakeup : BattleUnitBuf
    {
        public override float SpeedChange()
        {
            return 1.3f;
        }
    }
}
