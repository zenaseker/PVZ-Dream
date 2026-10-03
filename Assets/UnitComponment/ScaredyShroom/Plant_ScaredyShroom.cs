using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_ScaredyShroom : PlantBase
{
    bool haszombie = false;
    protected override void OnPlantUpdate()
    {
        haszombie = false;
        foreach (var monster in Physics2D.OverlapCircleAll(transform.position, 2.2f))
        {
            if (monster.gameObject.tag == "Zombie" && !monster.gameObject.GetComponent<ZombiesBase>().Reverse)
            {
                haszombie = true;
            }
        }
        if (haszombie) 
        {
            overhaszombie();
        }
        this.GetComponent<Animator>().SetBool("NearZombie", haszombie);
    }

    bool RayHit()
    {
        RaycastHit2D raycastHit2D = BattleManager.GetRay(this.transform.position, Vector2.right, MapManage.Instance.meshpos[0, MapManage.Instance.meshxy.y - 1].x + 4f - this.transform.position.x, 2);
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
    public virtual void overhaszombie()
    {

    }

    public override void OnAttack()
    {
        base.OnAttack();
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "PuffShroom_Default", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Default));
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
}
