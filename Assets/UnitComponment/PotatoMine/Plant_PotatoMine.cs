using UnityEngine;

public class Plant_PotatoMine : PlantBase
{
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        BuildTime = 14f;
        isBuild = false;
        base.Init(xy, plant);
    }
    public override void OnZombieContact(ZombiesBase zombies)
    {
        if (!isBuild || zombies.IgnoreSpecialAttack.Contains(IgonrePlant.Potato)) return;
        Broom();
    }

    public override void Building()
    {
        this.GetComponent<Animator>().SetTrigger("Build");
        MusicManage.Instance.PlayEffect("dirt_rise", 1f); 
    }
    public virtual void AfterBuild()
    {
        this.isBuild = true;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 1f, 2);
        if (collider2Ds != null && collider2Ds.Length > 0)
        {
            Broom();
        }
    }
    public virtual void Broom()
    {
        this.GetComponent<Animator>().SetTrigger("Broom");
        MusicManage.Instance.PlayEffect("potato_mine", 1f); 
        this.GetComponent<BoxCollider2D>().enabled = false;
        foreach (var monster in Physics2D.OverlapCircleAll(transform.position, 1f, 2))
        {
            if (monster.gameObject.tag == "Zombie" && monster.GetComponent<ZombiesBase>().Line == this.XY.x)
            {
                monster.GetComponent<ZombiesBase>().TakeDamage(new DamageObject(1800,Bullettype.PotatoMine, this)
                {
                    IgnoreArmor = true,
                }); ;
            }
        }
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "PotatoMineBroom", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(1f);
        Invoke("Die", 0.2f);
    }
}
