
using UnityEngine;

public class SnowFumeShroomEnchantment : EnchantmentBase
{
    public override void Enchantemnt(BattleUnitModel battleUnitModel, int dmg)
    {
        if (battleUnitModel is ZombiesBase) battleUnitModel.bufDetail.AddKeyWordBuf(KeyWordBuf.Cold,2,5f);
    }
}