using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IceRecoverEnchantment : EnchantmentBase
{
    public override void Enchantemnt(BattleUnitModel battleUnitModel, int dmg)
    {
        if (battleUnitModel is PlantBase && ((PlantBase)battleUnitModel).unitInfo.DreamElement.Contains(Attribute.DreamElement.Ice))
        {
            ((PlantBase)battleUnitModel).ReCoverHp(100); 
        }
    }
}