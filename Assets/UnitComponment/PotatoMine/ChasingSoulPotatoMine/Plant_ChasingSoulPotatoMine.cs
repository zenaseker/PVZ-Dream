using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_ChasingSoulPotatoMine : Plant_PotatoMine
{
    public class BattleUnitBuf_ChasingSoulPossess : BuffManage.BattleUnitBuf_Possess
    {
        public override PossessType possessType
        {
            get
            {
                return PossessType.Potato;
            }
        }
        public override int BrokenNum => 4;
        public override void OnTakeDamage(int dmg, DamageElement damageType)
        {
            base.OnTakeDamage(dmg, damageType);
            WakeUpNum++;
            if (WakeUpNum >= BrokenNum)
            {
                WakeUpNum = 0;
                MusicManage.Instance.PlayEffect("potato_mine", 1f);
                foreach (var monster in Physics2D.OverlapCircleAll(this._owner.transform.position, 1f, 2))
                {
                    if (monster.gameObject.tag == "Zombie" && monster.GetComponent<ZombiesBase>().bufDetail.GetKeyWordBuf(KeyWordBuf.MindControl) == null)
                    {
                        monster.GetComponent<ZombiesBase>().TakeDamage(new DamageObject(300, Bullettype.PotatoMine, null)
                        {
                            DamageElement = DamageElement.Soul,
                        });
                    }
                }
                GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "ChasingSoulPotatoMineBroom", this._owner.transform.position);
                RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(1f);
            }
        }
    }
    bool inmove = false;
    ZombiesBase targetzombie = null;
    bool inzombie = false;
    public override void OnZombieContact(ZombiesBase zombies)
    {
        if (!isBuild || inmove || zombies.IgnoreSpecialAttack.Contains(IgonrePlant.Potato)) return;
        Broom();
    }
    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        if (!isBuild || inzombie) return;
        if (inmove)
        {
            if (targetzombie != null)
            {
                if (Vector3.Distance(this.transform.position,targetzombie.transform.position) > 0.1f)
                {
                    Vector3 dir = (this.transform.position - targetzombie.transform.position).normalized * 0.1f;
                    this.transform.position -= dir;
                    return;
                }
                this.transform.position = targetzombie.transform.position;
                inzombie = true;
                Broom();
                return;
            }
            Broom();
            return;
        }
        float x = 99f;
        ZombiesBase _zombie = null;
        foreach (ZombiesBase zombie in ZombieManage.Instance.Zombies)
        {
            if (zombie != null && !zombie.Reverse)
            {
                if (zombie.transform.position.x < x)
                {
                    x = zombie.transform.position.x;
                    _zombie = zombie;
                }
            }
        }
        if (_zombie != null)
        {
            this.GetComponent<Animator>().SetFloat("FindZombie", x <= this.transform.position.x ?- 1:1);
            targetzombie = _zombie;
        }
    }
    public void CanMove()
    {
        inmove = true;
    }
    public override void Building()
    {
        base.Building();
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
    public override  void AfterBuild()
    {
        this.isBuild = true;
    }
    public override void Broom()
    {
        this.GetComponent<Animator>().SetTrigger("Broom");
        MusicManage.Instance.PlayEffect("potato_mine", 1f);
        this.GetComponent<BoxCollider2D>().enabled = false;
        foreach (var monster in Physics2D.OverlapCircleAll(transform.position, 1f, 2))
        {
            if (monster.gameObject.tag == "Zombie")
            {
                monster.GetComponent<ZombiesBase>().TakeDamage(new DamageObject(1800, Bullettype.PotatoMine, this)
                {
                    DamageElement = DamageElement.Soul,
                });
            }
        }
        foreach (var monster in Physics2D.OverlapCircleAll(this.transform.position, 2f, 2))
        {
            if (monster.gameObject.tag == "Zombie" && monster.GetComponent<ZombiesBase>().bufDetail.GetKeyWordBuf(KeyWordBuf.MindControl) == null)
            {
                ZombiesBase zombie = monster.GetComponent<ZombiesBase>();
                if (zombie.bufDetail.GetBufList().Find(x => x is BattleUnitBuf_ChasingSoulPossess) == null)
                {
                    if (zombie.bufDetail.GetKeyWordBuf(KeyWordBuf.Possess) != null)
                    {
                        zombie.bufDetail.RemoveBuf(x => x.KeyWordBuf == KeyWordBuf.Possess);
                    }
                    zombie.bufDetail.AddBuf(new BattleUnitBuf_ChasingSoulPossess());
                }
            }
        }
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "ChasingSoulPotatoMineBroom", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(1f);
        Invoke("Die", 0.1f);
    }
}
