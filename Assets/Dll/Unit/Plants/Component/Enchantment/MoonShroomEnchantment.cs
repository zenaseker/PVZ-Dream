
public class MoonShroomEnchantment : EnchantmentBase
{
    public override void Enchantemnt(BattleUnitModel battleUnitModel, int dmg)
    {
        if (battleUnitModel is ZombiesBase) BattleManage.CreateSun(battleUnitModel.gameObject.transform.position, 1, true);
    }
}