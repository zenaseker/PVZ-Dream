using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_PeaShooter : Zombie_Common
{
    public Transform atkpos;
    public void PeaAttack()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "PeaBullet", atkpos.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(this.Reverse ? 5.5f : -5.5f, 0), this.Line,this.Reverse ? UnitFaction.Plant: UnitFaction.Zombie, this.Damage(20, DamageElement.Default));
    }
    public override void LoseHead()
    {
        this.losehead = true;
        if (isdie) return;
        Invoke("DieAfterLoseHead", 2f);
    }
}
