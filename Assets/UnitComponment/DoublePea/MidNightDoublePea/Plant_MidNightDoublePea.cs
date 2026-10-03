using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_MidNightDoublePea : Plant_DoublePea
{
    public override void OnAttack()
    {
        if (this.component._matrix._RegetMatrixnum > 1)
        {
            GameObject gameObject2 = PoolManage.Instance.GetPoolGameObject("Bullet", "NightPeaBullet", ObjCreateTsf.position);
            gameObject2.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage * 2, DamageElement.Default));
            InitBullet(gameObject2.GetComponent<IEnchantment>());
        }
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "DarkPeaBullet", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Default));
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
}
