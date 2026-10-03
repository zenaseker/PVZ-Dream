using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkFrostStream : FrostStream
{
    int dmg;
    public void Init(int damage,float area)
    {
        base.Init(area, area);
        dmg = damage;
    }
    public override void Hits(float area)
    {
        base.Hits(area);
        MusicManage.Instance.PlayEffect("Boom", 1);
        foreach (var monster in Physics2D.OverlapCircleAll(transform.position, area))
        {
            if (monster.gameObject.tag == "Zombie" && !monster.GetComponent<ZombiesBase>().Reverse)
            {
                monster.GetComponent<ZombiesBase>().TakeDamage(new DamageObject(this.dmg, Bullettype.Icicle, null)
                {
                    DamageElement = DamageElement.Snow
                });
            }
        }
    }
}
