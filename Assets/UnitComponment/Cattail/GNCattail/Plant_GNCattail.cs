using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class Plant_GNCattail : PlantBase
{
    bool haszombie = false;
    ZombiesBase target = null;
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
        this.GetComponent<Animator>().SetBool("NearZombie", haszombie);
        Attackinterval = haszombie ? 5 : 2.5f;
    }

    bool RayHit()
    {
        float dis = 99f;
        ZombiesBase zombie = null;
        foreach(ZombiesBase zombies in ZombieManage.Instance.Zombies)
        {
            if (zombies != null && zombies.transform.position.x < dis)
            {
                dis = zombies.transform.position.x;
                zombie = zombies;
            }
        }
        if (zombie != null)
        {
            target = zombie;
            return true;
        }
        return false;
    }
    protected override bool CanAttack()
    {
        return base.CanAttack() && RayHit();
    }

    public override void OnAttack()
    {
        base.OnAttack();
        if (haszombie)
        {
            for(int i = 0;i < 6; i++)
            {
                GameObject gameObject1 = PoolManage.Instance.GetPoolGameObject("Bullet", "GNCattail_bullet 1", this.transform.position);
                gameObject1.GetComponent<BulletBase>().Init(new Vector2((float)Math.Cos(i * 60 * Mathf.Deg2Rad), (float)Math.Sin(i * 60 * Mathf.Deg2Rad)) * 5.5f, -1, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Default));
                gameObject1.GetComponent<BulletBase>().attacker = this;
                gameObject1.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, i * 60);
            }
            return;
        }
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "GNCattail_bullet", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), -1, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Default));
        gameObject.GetComponent<BulletBase>().attacker = this;
        gameObject.GetComponent<BulletBase>().target = target;
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
}
