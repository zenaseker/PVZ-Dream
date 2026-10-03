using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIrePeaBullet : PeaBullet
{
    public override void OnHit(BattleUnitModel unit)
    {
        if (unit.gameObject.tag == "Zombie")
        {
            foreach (var monster in Physics2D.OverlapCircleAll(transform.position, 0.7f, 2))
            {
                if (monster.gameObject.tag == "Zombie" && monster != unit)
                {
                    monster.GetComponent<ZombiesBase>().TakeDamage(new DamageObject(13, type, attacker)
                    {
                        DamageElement = DamageElement.Fire,
                    });
                }
            }
        }
        MusicManage.Instance.PlayEffect("firepea", 1);
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "FirePS", transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.22f);
        base.OnHit(unit);
    }
}
