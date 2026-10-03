
public class DarkFrostEnchantment : EnchantmentBase
{
    public override void Enchantemnt(BattleUnitModel battleUnitModel, int dmg)
    {
        if (battleUnitModel is ZombiesBase) battleUnitModel.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 1);
    }
}