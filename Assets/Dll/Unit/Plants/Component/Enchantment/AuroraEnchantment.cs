public class AuroraEnchantment : EnchantmentBase
{
    public override void Enchantemnt(BattleUnitModel battleUnitModel, int dmg)
    {
        if (battleUnitModel is ZombiesBase) ((ZombiesBase)battleUnitModel).TakeDamage(new DamageObject(10,Bullettype.Icicle, this._detail._owner)
        {
            DamageElement = DamageElement.Snow,
            IgnoreAromor2 = true,
        }); ;
    }
}