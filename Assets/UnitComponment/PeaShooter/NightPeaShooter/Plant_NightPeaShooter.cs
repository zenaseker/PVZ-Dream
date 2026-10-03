using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_NightPeaShooter : Plant_PeaShooter
{
    public override void OnAttack()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "NightPeaBullet",ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Dark));
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
}
