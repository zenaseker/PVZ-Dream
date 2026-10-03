
using UnityEngine;

public class MoonErosionEnchantment : EnchantmentBase
{
    public override void Enchantemnt(BattleUnitModel battleUnitModel, int dmg)
    {
        if (battleUnitModel is ZombiesBase && this._detail._owner.posType == PlantPosType.Little && Random.Range(0f, 1f) < 0.5f) battleUnitModel.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion,1);
    }
}