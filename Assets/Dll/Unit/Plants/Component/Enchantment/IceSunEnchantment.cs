using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IceSunEnchantment : EnchantmentBase
{
    public override void Enchantemnt(BattleUnitModel battleUnitModel, int dmg)
    {
        if (battleUnitModel is ZombiesBase) BattleManage.CreateSun(battleUnitModel.gameObject.transform.position, 5, true);
    }
}