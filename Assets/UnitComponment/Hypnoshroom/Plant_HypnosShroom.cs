using UnityEngine;

public class Plant_HypnosShroom : PlantBase
{
    public override void OnTakeDamage(int dmg, BattleUnitModel attacker, DamageElement damageType = DamageElement.Default)
    {
        if (attacker is ZombiesBase)
        {
            attacker.bufDetail.AddBuf(new BuffManage.BattleUnitBuf_Hypnos(),1);
            this.OnControl((ZombiesBase)attacker);
            this.Destory();
        }
    }
    public virtual void OnControl(ZombiesBase zombie) { }
}
