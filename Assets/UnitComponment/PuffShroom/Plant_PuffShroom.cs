using UnityEngine;

public class Plant_PuffShroom : PlantBase
{
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
    }
    public override void OnAttack()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "PuffShroom_Default", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Default));
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
    bool RayHit()
    {
        RaycastHit2D raycastHit2D = BattleManager.GetRay(this.transform.position, Vector2.right, 6, 2);
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
