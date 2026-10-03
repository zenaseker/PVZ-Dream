using UnityEngine;

public class Plant_MeteorFlower : Plant_SunFlower
{
    int productcount = 0;
    public override void Product()
    {
        base.Product();
        productcount++;
        if (productcount >= 4)
        {
            this.GetComponent<Animator>().SetFloat("Attack", 1);
            productcount = 0;
        }
        else
        {
            smallattack();
        }
    }
    public void AfterAttack()
    {
        this.GetComponent<Animator>().SetFloat("Attack", 0);
    }
    void smallattack()
    {
        ZombiesBase zombies = null;
        ZombiesBase zombies1 = null;
        ZombiesBase zombies2 = null;
        float x = 99f;
        foreach (ZombiesBase zombie in ZombieManage.Instance.Zombies)
        {
            if (zombie != null && !zombie.Reverse)
            {
                if (zombie.transform.position.x < x)
                {
                    zombies2 = zombies1;
                    zombies1 = zombies;
                    zombies = zombie;
                    x = zombie.transform.position.x;
                }
            }
        }
        if (zombies != null)
        {
            GameObject bullet = PoolManage.Instance.GetPoolGameObject("Bullet", "MeteorSmallBullet", ObjCreateTsf.position);
            bullet.GetComponent<MeteorSmallBullet>().Init(Vector2.right, this.XY.x, UnitFaction.Plant);
            bullet.GetComponent<MeteorSmallBullet>().target = zombies;
            bullet.GetComponent<MeteorSmallBullet>().attacker = this;
        }
        if (zombies1 != null)
        {
            GameObject bullet = PoolManage.Instance.GetPoolGameObject("Bullet", "MeteorSmallBullet", ObjCreateTsf.position);
            bullet.GetComponent<MeteorSmallBullet>().Init(Vector2.right, this.XY.x, UnitFaction.Plant);
            bullet.GetComponent<MeteorSmallBullet>().target = zombies1;
            bullet.GetComponent<MeteorSmallBullet>().attacker = this;
        }
        if (zombies2 != null)
        {
            GameObject bullet = PoolManage.Instance.GetPoolGameObject("Bullet", "MeteorSmallBullet", ObjCreateTsf.position);
            bullet.GetComponent<MeteorSmallBullet>().Init(Vector2.right, this.XY.x, UnitFaction.Plant);
            bullet.GetComponent<MeteorSmallBullet>().target = zombies2;
            bullet.GetComponent<MeteorSmallBullet>().attacker = this;
        }
    }
    public void Fight()
    {
        Vector3 pos = new Vector3(10,0,0);
        foreach (ZombiesBase zombie in ZombieManage.Instance.Zombies)
        {
            if (zombie != null && !zombie.Reverse && zombie.transform.position.x < pos.x)
            {
                pos = zombie.transform.position;
            }
        }
        PoolManage.Instance.GetPoolGameObject("Bullet", "Meteor",pos);
    }
}
