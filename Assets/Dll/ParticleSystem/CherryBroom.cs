using UnityEngine;

public class CherryBroom : ParitcleSystemBase
{
    public int dmg = 1800;
    public bool isbroom = true;
    public void Init(int damage, bool Broom,float area, float time = 10)
    {
        base.Init(area,area, time);
        dmg = damage;
        isbroom = Broom;
    }
    public override void Hits(float area)
    {
        base.Hits(area);
        MusicManage.Instance.PlayEffect("cherrybomb", 1);
        foreach (var monster in Physics2D.OverlapCircleAll(transform.position, area))
        {
            if (monster.gameObject.tag == "Zombie" && !monster.GetComponent<ZombiesBase>().Reverse)
            {
                monster.GetComponent<ZombiesBase>().TakeDamage(new DamageObject(this.dmg,Bullettype.Cherry, null)
                {
                    IsBroom = true
                });
                Hit(monster.GetComponent<ZombiesBase>(), dmg, DamageElement.Default, false, isbroom);
            }
            if (monster.gameObject.tag == "Plant")
            {
                Hit(monster.GetComponent<PlantBase>(), dmg, DamageElement.Default, false, isbroom);
            }
        }
    }
    public override void Hit(BattleUnitModel model, int d, DamageElement damageType, bool ignorearmor = false,bool isbroom = false)
    {
        base.Hit(model,d,damageType);
    }
}
