using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IceblastEnchantment: EnchantmentBase
{

    public override void Enchantemnt(BattleUnitModel battleUnitModel, int dmg)
    {
        if (battleUnitModel is ZombiesBase) battleUnitModel.bufDetail.AddBuf(new Icyblast(),1);
    }
    public class Icyblast : BattleUnitBuf
    {
        public override void OnUpdate(float deltatime)
        {
            base.OnUpdate(deltatime);
            if (this._owner.bufDetail.GetKeyWordBuf(KeyWordBuf.Cold) == null)
            {
                this.Destory();
            }
        }
        public override float TakeDamageChange(int dmg,DamageElement damageType)
        {
            return 1.5f;
        }
    }
}