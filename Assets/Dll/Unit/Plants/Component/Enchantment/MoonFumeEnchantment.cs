
using UnityEngine;

public class MoonFumeEnchantment : EnchantmentBase
{
    public override void Enchantemnt(BattleUnitModel battleUnitModel, int dmg)
    {
        float num = 0.5f;
        if (this._detail._matrix != null && this._detail._matrix is MoonFumeMatrix)
        {
            num += (float)(this._detail._matrix._RegetMatrixnum * 0.04);
        }
        if (battleUnitModel is ZombiesBase && Random.Range(0f, 1f) < num) battleUnitModel.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 1);
    }
}