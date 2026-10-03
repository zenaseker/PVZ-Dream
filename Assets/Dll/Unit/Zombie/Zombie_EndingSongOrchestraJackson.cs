using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuffManage;

public class Zombie_EndingSongOrchestraJackson : Zombie_Jackson
{
    float time = 0f;
    bool firstup = true;
    int login = 0;
    Dancerreverb[] dancerreverbs = null;
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed = 1)
    {
        base.Init(order, zombieCard, line, startspeed);
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (time > 300f)
        {
            return;
        }
        if (collision.gameObject.tag == "Plant" && !collision.gameObject.GetComponent<PlantBase>().IgnoreUnit && collision.gameObject.GetComponent<PlantBase>().posType != PlantPosType.Top)
        {
            PlantBase plantbase = collision.gameObject.GetComponent<PlantBase>();
            plantbase.OnZombieContact(this);
            OnPlantContact(plantbase);
        }
    }
    public void CheckLogin()
    {
        float i = Random.Range(0f, 1f);
        if (time > 100f && i < 0.5f)
        {
            login = 5;
            this.GetComponent<Animator>().SetBool("ToWalk",true);
        }
        else if (i < 0.35f)
        {
            login = 1;
            this.GetComponent<Animator>().SetBool("ToWalk", false);
        }
        else if (i < 0.6f)
        {
            login = 2;
            this.GetComponent<Animator>().SetBool("ToWalk", false);
        }
        else if (i < 0.75f)
        {
            login = 3;
            this.GetComponent<Animator>().SetBool("ToWalk", false);
        }
        else
        {
            login = 4;
            this.GetComponent<Animator>().SetBool("ToWalk", false);
        }
    }
    protected override void AnimSummon()
    {
        if (firstup)
        {
            firstup = false;
            dancerreverbs = new Dancerreverb[MapManage.Instance.meshxy.x];
            for (int i = 0;i < MapManage.Instance.meshxy.x; i++)
            {
                GameObject Dancerreverb = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "Dancerreverb", MapManage.Instance.meshpos[i,MapManage.Instance.meshxy.y - 1],null);
                Dancerreverb dancerreverb = Dancerreverb.GetComponent<Dancerreverb>();
                dancerreverb.Init(i);
                dancerreverbs[i] = dancerreverb;
            }
        }
        else
        {
            switch (login)
            {
                case 1:
                    if (this.Line != 0)
                    {
                        CreateDancer(0, 8);
                        CreateDancer(4, 8);
                        CreateDancer(5, 8);
                    }
                    CreateDancer(1, 8);
                    CreateDancer(2, 8);
                    if (this.Line != MapManage.Instance.meshxy.x - 1)
                    {
                        CreateDancer(3, 8);
                        CreateDancer(6, 8);
                        CreateDancer(7, 8);
                    }
                    PoolManage.Instance.GetPoolGameObject("ParticleSystem", "DarkImpact").transform.localScale = Vector3.one * 5f;
                    foreach (var monster in Physics2D.OverlapCircleAll(this.transform.position, 5f, 1 << LayerMask.NameToLayer("Unit")))
                    {
                        if (monster.gameObject.tag == "Plant")
                        {
                            DamageObject damageObject = new DamageObject(50, Bullettype.Zombie, this)
                            {
                                DamageElement = DamageElement.Dark,
                            };
                            monster.GetComponent<PlantBase>().TakeDamage(damageObject);
                            monster?.GetComponent<PlantBase>()?.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 3);
                        }
                    }
                    break;
                case 2:
                    foreach(Dancerreverb d in dancerreverbs)
                    {
                        d.Create().bufDetail.AddBuf(new BattleUnitBuf_JacksonPossess(), 1);
                    }
                    break;
                case 3:
                    if (this.Line != 0)
                    {
                        CreateDancer(0, 7);
                    }
                    CreateDancer(1, 7);
                    CreateDancer(2, 7);
                    if (this.Line != MapManage.Instance.meshxy.x - 1)
                    {
                        CreateDancer(3, 7);
                    }
                    this.ReCoverHp((int)(BattleManage.Instance.SunNumber / 100 * 0.5f * this.MaxHP));
                    break;
                case 4:
                    if (this.Line != 0)
                    {
                        CreateDancer(0, 7);
                    }
                    CreateDancer(1, 111);
                    CreateDancer(2, 407);
                    if (this.Line != MapManage.Instance.meshxy.x - 1)
                    {
                        CreateDancer(3, 307);
                    }
                    break;
            }
        }
    }
    protected override void OnUpdate()
    {
        time += Time.deltaTime;
        if (isMoonWalkFinish) return;
        moonWalkTime += Time.deltaTime;
        if (moonWalkTime > 5f && !isMoonWalkFinish)
        {
            this.GetComponent<Animator>().SetTrigger("summon");
            isMoonWalkFinish = true;
        }
    }
    protected void CreateDancer(int summon, int id = 8)
    {
        GameObject obj = ZombieManage.Instance.InitZombie(id, this.Line);
        switch (summon)
        {
            case 0://下
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line - 1, this.speed);
                obj.transform.position = this.transform.position + Vector3.down * 2;
                break;
            case 1://左
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line, this.speed);
                obj.transform.position = this.transform.position + Vector3.left * 2;
                break;
            case 2://右
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line, this.speed);
                obj.transform.position = this.transform.position + Vector3.right * 2;
                break;
            case 3://上
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line + 1, this.speed);
                obj.transform.position = this.transform.position + Vector3.up * 2;
                break;
            case 4://左下
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line - 1, this.speed);
                obj.transform.position = this.transform.position + new Vector3(2,-2,0);
                break;
            case 5://右下
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line - 1, this.speed);
                obj.transform.position = this.transform.position + new Vector3(-2, -2, 0);
                break;
            case 6://左上
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line + 1, this.speed);
                obj.transform.position = this.transform.position + new Vector3(2, 2, 0);
                break;
            case 7://右上
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line + 1, this.speed);
                obj.transform.position = this.transform.position + new Vector3(-2, 2, 0);
                break;
        }
        obj.GetComponent<ZombiesBase>().ZombieUp();
    }
    public override bool CanAddBuf(BattleUnitBuf buf)
    {
        return buf.KeyWordBuf != KeyWordBuf.Cold && buf.KeyWordBuf != KeyWordBuf.MindControl && base.CanAddBuf(buf);
    }
    public class BattleUnitBuf_JacksonPossess: BattleUnitBuf_Possess
    {
        public override PossessType possessType
        {
            get
            {
                return PossessType.Jackson;
            }
        }

        public override void OnDie()
        {
            base.OnDie();
            foreach (var monster in Physics2D.OverlapCircleAll(_owner.transform.position, 1f, 1 << LayerMask.NameToLayer("Unit")))
            {
                if (monster.gameObject.tag == "Plant")
                {
                    DamageObject damageObject = new DamageObject(100, Bullettype.Zombie, _owner)
                    {
                        DamageElement = DamageElement.Soul,
                    };
                    monster.GetComponent<PlantBase>().TakeDamage(damageObject);
                }
            }
        }

    }
}
