
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Attribute;

public class Plant_SnowPea : Plant_PeaShooter
{
    public override void OnAttack()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "SnowPeaBullet", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), (int)this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Snow));
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
    }
}
