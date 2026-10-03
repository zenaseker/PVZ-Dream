
using UnityEngine;

public class Plant_PeaShooter : PlantBase
{

    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
    }
    public override void OnAttack()
    {
        if (BattleManage.Instance.level.KeyImage == "FU")
        {
            GameObject gameObject2 = PoolManage.Instance.GetPoolGameObject("Bullet", "FuBullet", ObjCreateTsf.position);
            gameObject2.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Default));
            InitBullet(gameObject2.GetComponent<IEnchantment>());
        }
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "PeaBullet", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Default));
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
    bool RayHit()
    {
        RaycastHit2D raycastHit2D = BattleManager.GetRay(this.transform.position, Vector2.right, MapManage.Instance.meshpos[0,MapManage.Instance.meshxy.y - 1].x + 4f - this.transform.position.x, 2);
        if (raycastHit2D)
        {
            GameObject ray = raycastHit2D.transform.gameObject;
            if (ray.tag == "Zombie" && !ray.GetComponent<ZombiesBase>().IgnoreSpecialAttack.Contains(IgonrePlant.LineAttacker))
            {
                return true;
            }
        }
        return false;
    }
    protected override bool CanAttack()
    {
        return base.CanAttack() && RayHit();
    }
}
