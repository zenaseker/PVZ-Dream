using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_ProtoPeaShooter : Plant_PeaShooter
{
    public override void OnAttack()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "ProtoPeaBullet", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Default));
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
}
