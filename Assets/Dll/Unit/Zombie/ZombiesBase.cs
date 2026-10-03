using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using static Attribute;


public enum ZombieState
{
    Default,//站立
    Walk,//行走
    Attack,//攻击
    Walk2,//行走2
    Attack2,//攻击2
    Die,//死亡
    Run,//奔跑
    Jump,//跳跃
    LosePaper,//读报失去报纸
    Fall,//坠落
}

public enum IgonrePlant
{
    LineAttacker,//直线攻击植物(豌豆、蘑菇)
    Chomper,//大嘴花
    Potato//土豆地雷

}

public enum DamageKey//特殊伤害方式
{
    IgnoreArmor,//无视护具
    Broom,//灰烬伤害

}

public class ZombiesBase : BattleUnitModel
{
    public Armor armor1 = null;
    public Armor armor2 = null;
    public Armor armor3 = null;
    public Rigidbody2D rigidbody2d;

    [Header("正在攻击的单位")]
    public BattleUnitModel unitbase;
    protected List<BattleUnitModel> Units = new List<BattleUnitModel>();
    public bool losehead = false;
    public bool losearm = false;
    public bool isdie = false;
    public int Line;
    public List<IgonrePlant> IgnoreSpecialAttack = new List<IgonrePlant>();//无视特殊攻击(大嘴、地雷等)
    public bool Reverse = false;//反向
    public virtual void Init(int order,Attribute.ZombieInfo zombieCard, int line,float startspeed = 1f)
    {
        this.unitInfo = zombieCard.Clone();
        this.bufDetail = new UnitBufDetail(this);
        if (BattleManage.Instance.LevelDreamDepth > 0)
        {
            foreach(BattleUnitBuf zombieBufBase in BuffManage.Instance.GetZombieDreamDepthIncrease())
            {
                this.bufDetail.AddBuf(zombieBufBase);
            }
        }
        HP = MaxHP;
        Line = line;
        this.speed = UnityEngine.Random.Range(0.9f + this.bufDetail.StartSpeedMin, 1.1f + this.bufDetail.StartSpeedMax);
        if (startspeed != 1)
        {
            this.speed = startspeed;
        }
        this.ChangeSpeed();
        this.GetComponent<SortingGroup>().sortingOrder = order;
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            if (spriteRenderer.gameObject.name != "shadow")
            {
                spriteRenderers.Add(spriteRenderer);
            }
        }
        armor1?.Init(this);
        armor2?.Init(this);
        armor3?.Init(this);
        if (BattleManage.Instance.battleStage >= BattleStage.InBattle)
        {
            this.GetComponent<Animator>().SetBool("Go", true);
        }
    }

    protected override void OnUpdate()
    {
        if (BattleManage.Instance.battleStage < BattleStage.InBattle)
        {
            return;
        }
        bufDetail.OnUpdate?.Invoke(Time.deltaTime);
        if (this.HP < 0)
        {
            DieAfterLoseHead();
        }
    }


    public override void ColorChange()
    {
        base.ColorChange();
        if (armor1 != null)
        {
            armor1.aromorspr.color = bufDetail.GetColor();
        }
        if (armor2 != null)
        {
            armor2.aromorspr.color = bufDetail.GetColor();
        }
        if (armor3 != null)
        {
            armor3.aromorspr.color = bufDetail.GetColor();
        }
    }
    public override void TakeDamage(DamageObject damageObject)
    {
        if (unitInfo?.ID / 10000 > 0 && damageObject.Damage > 4000)
        {
            damageObject.Damage = 4000;
        }
        damageObject.Damage = (int)(damageObject.Damage * this.bufDetail.TakeDamageChange(damageObject.Damage, damageObject.DamageElement));
        bufDetail.OnTakeDamage?.Invoke(damageObject.Damage, damageObject.DamageElement);
        if(Attribute.Instance.filedInfo.Difficulty >= 3 || this.MaxHP > 1800)
        {
            damageObject.IsBroom = false;
        }
        if (damageObject.IgnoreArmor || damageObject.DamageElement == DamageElement.Soul)
        {
            goto IgnoreArmor;
        }
        if (damageObject.IsBroom)
        {
            goto IgnoreArmor;
        }
        if (this.armor3 != null)
        {
            this.armor3.TakeDamage(damageObject);
            return;
        }
        if (this.armor2 != null && !damageObject.IgnoreAromor2)
        {
            this.armor2.TakeDamage(damageObject);
            return;
        }
        if (this.armor1 != null)
        {
            this.armor1.TakeDamage(damageObject);
            return;
        }
        if (this.HP <= 0) return;
        try
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.material.DOFloat(1.3f, "_HighLight", 0.05f).onComplete += () =>
                    {
                        if (spriteRenderer != null)
                        {
                            spriteRenderer?.material.DOFloat(1f, "_HighLight", 0.05f);
                        }
                    };
                }
            }
        }
        catch
        {

        }
    IgnoreArmor:
        this.HP -= damageObject.Damage;
        if (damageObject.IsBroom)
        {
            this.DieByBroom();
            return;
        }
        if (this.HP * 3 < this.MaxHP * 2 && !this.losearm)
        {
            this.LoseArm();
        }
        if (this.HP * 3 < this.MaxHP && !this.losehead)
        {
            this.LoseHead();
            damageObject.OriginUnit?.Kill(this);
        }
        if (this.HP < 0 && this.losehead)
        {
            DieAfterLoseHead();
        }
    }
    public virtual void OnLoseArmor(Armor armor)
    {

    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Plant" && !collision.gameObject.GetComponent<PlantBase>().IgnoreUnit && collision.gameObject.GetComponent<PlantBase>().posType != PlantPosType.Top)
        {
            PlantBase plantbase = collision.gameObject.GetComponent<PlantBase>();
            if (plantbase is Plant_WallNutBall)
            {
                plantbase.OnZombieContact(this);
                return;
            }
            Units.Add(plantbase);
            if (this.HP >= 0 && !this.losehead)
            {
                unitbase = plantbase;
                plantbase.OnZombieContact(this);
                OnPlantContact(plantbase);
                this.GetComponent<Animator>().SetBool("Attack", true);
            }
        }
        if (collision.gameObject.tag == "Zombie" && (this.Reverse || collision.gameObject.GetComponent<ZombiesBase>().Reverse))
        {
            Units.Add(collision.gameObject.GetComponent<BattleUnitModel>());
            if (this.HP >= 0 && !this.losehead)
            {
                unitbase = collision.gameObject.GetComponent<BattleUnitModel>();
                this.GetComponent<Animator>().SetBool("Attack", true);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Plant" && Units.Contains(collision.gameObject.GetComponent<PlantBase>()))
        {
            if (collision.gameObject.GetComponent<PlantBase>() is Plant_WallNutBall)
            {
                return;
            }
            Units.Remove(collision.gameObject.GetComponent<BattleUnitModel>());
        }
        if (collision.gameObject.tag == "Zombie" && Units.Contains(collision.gameObject.GetComponent<ZombiesBase>()))
        {
            Units.Remove(collision.gameObject.GetComponent<BattleUnitModel>());
        }
        if (unitbase != null && this.HP >= 0 && !this.losehead)
        {
            ChangeAttackUnit();
        }
    }
    public virtual void OnPlantContact(PlantBase plant)
    {

    }

    public virtual void OnMindControl()
    {

    }
    public void ChangeAttackUnit(float notime = 0.01f)
    {
        if (Units.Count > 0)
        {
            float dis = 99f;
            BattleUnitModel atkplant = null;
            foreach (BattleUnitModel unit in Units)
            {
                if (Vector3.Distance(unit.transform.position, this.transform.position) < dis)
                {
                    atkplant = unit;
                    dis = Vector3.Distance(unit.transform.position, this.transform.position);
                }
            }
            if (atkplant != null)
            {
                unitbase = atkplant;
                this.GetComponent<Animator>().SetBool("Attack", true);
            }
            return;
        }
        unitbase = null;
        this.GetComponent<Animator>().SetBool("Attack", false);
    }
    public void ZombieUp()
    {
        this.GetComponent<Collider2D>().enabled = false;
        MusicManage.Instance.PlayEffect("dirt_rise", 1);
        PoolManage.Instance.GetPoolGameObject("ParticleSystem", "ZombieUpMask", this.transform);
        foreach(SpriteRenderer spriteRenderer in this.transform.Find("Body").GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        this.transform.Find("Body").localPosition += Vector3.down * 2;
        this.transform.Find("Body").DOLocalPath(new Vector3[] { this.transform.Find("Body").localPosition , this.transform.Find("Body").localPosition + Vector3.up * 2 }, 1f).onComplete += AfterZombieUp;
    }
    private void AfterZombieUp()
    {
        this.GetComponent<Collider2D>().enabled = true;
        this.GetComponent<Animator>().SetBool("Go", true);
        GameObject mask = this.transform.Find("ZombieUpMask").gameObject;
        PoolManage.Instance.PushGameObject(mask.name, mask,true);
        foreach (SpriteRenderer spriteRenderer in this.transform.Find("Body").GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
    }
    public virtual void Attack()
    {
        if (this.losehead) return;
        if (unitbase == null)
        {
            this.GetComponent<Animator>().SetBool("Attack", false);
            return;
        };
        DamageObject damageObject = new DamageObject(this.Damage(0, DamageElement.Default), Bullettype.Zombie, this);
        bufDetail.OnAttack?.Invoke(damageObject);
        unitbase.TakeDamage(damageObject);
        MusicManage.Instance.PlayEffect(RandomUtil.SelectOne(Attribute.Instance.NormalMusic["ZombieAttack"]), 0.7f);
    }

    public void DestoryArmor(Armor armor)
    {
        if(armor.armortype == 3)
        {
            this.armor3 = null;
        }
        else if (armor.armortype == 2)
        {
            this.armor2 = null;
        }
        else
        {
            this.armor1 = null;
        }
        OnLoseArmor(armor);
    }
    public virtual void StartDie()//开始倒地
    {

    }
    public virtual void Die()//倒地动画结束
    {
        if (!this.losearm)
        {
            this.LoseArm();
        }
        if (!this.losehead)
        {
            this.LoseHead();
        }
        bufDetail.OnDie?.Invoke();
        bufDetail.OnDestory();
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.Destroy();
    }
    public void Destroy()//移除实体
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            DOTween.Kill(spriteRenderer);
        }
        DOTween.Kill(this.gameObject);
        if (BattleManage.Instance.battleStage != BattleStage.ChooseCard)
        {
            for(int i = 0;i < 256; i++)
            {
                if (ZombieManage.Instance.Zombies[i] == this)
                {
                    ZombieManage.Instance.Zombies[i] = null;
                    break;
                }
            }
            ZombieManage.Instance.LastZombiePos = this.transform.position;
            if (unitInfo?.ID / 10000 > 0) 
            {
                ZombieManage.Instance.AliveBoss.Remove(this);
                if (ZombieManage.Instance.AliveBoss.Count <= 0) ZombieManage.Instance.BossTime = false;
            }
        }
        GameObject.Destroy(this.gameObject);
    }
    public virtual void DieByBroom()//炸成灰
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "BroomBody", this.transform.position);
        if (this.Reverse)
        {
            obj.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            obj.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(3.5f);
        this.Destroy();
    }

    public virtual void LoseArm()//掉手
    {
        this.losearm = true;
    }

    public virtual void LoseHead()//掉头
    {
        this.losehead = true;
        if (isdie) return;
        Invoke("DieAfterLoseHead", 2f);
    }

    public virtual void DieAfterLoseHead()//掉头后开始倒地
    {
        if (isdie) return;
        isdie = true;
        this.GetComponent<Animator>().SetTrigger("Die");
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
    public override bool CanAddBuf(BattleUnitBuf buf)
    {
        if (buf.KeyWordBuf == KeyWordBuf.Cold && unitInfo.DreamElement.Contains(DreamElement.Ice))
        {
            return false;
        }
        return !this.losehead;
    }

    public void ZombieDieByCart()//被小推车碾压
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponent<Animator>().enabled = false;
        this.transform.DOLocalRotate(new Vector3(0f,0f,-90f),0.15f).SetEase(Ease.Linear).onComplete += () =>
        {
            if (this.transform != null)
            {
                this.transform.DOScale(new Vector3(1f, 0.1f, 1f), 0.15f).SetEase(Ease.Linear).onComplete += () =>
                {
                    this.Die();
                };
            }
        };
    }
}
