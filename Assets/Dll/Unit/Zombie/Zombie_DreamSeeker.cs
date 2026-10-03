using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_DreamSeeker : ZombiesBase
{
    private bool injump = false;
    public ZombiesBase Star = null;
    bool getStar = false;
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed = 1f)
    {
        base.Init(order, zombieCard, line,startspeed);
        if (BattleManage.Instance.battleStage >= BattleStage.InBattle)
        {
            GameObject obj = ZombieManage.Instance.InitZombie(104,this.Line);
            Star = obj.GetComponent<ZombiesBase>();
            ZombieManage.Instance.LoadZombieMess(obj, 104, line);
            obj.transform.position = new Vector3(Random.Range(MapManage.Instance.meshpos[0, 2].x, MapManage.Instance.meshpos[0, 4].x), MapManage.Instance.meshpos[line, 0].y - 0.75f);
            this.GetComponent<Animator>().SetBool("Go", true);
        }
        
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Star != null && this.transform.position.x < Star.transform.position.x)
        {
            Star.gameObject.GetComponent<ZombiesBase>().Destroy();
            OnGetStar();
        }
        if (Star == null && !getStar)
        {
            OnStarBreack();
        }
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Zombie" && collision.GetComponent<ZombiesBase>() == Star)
        {
            collision.gameObject.GetComponent<ZombiesBase>().Destroy();
            OnGetStar();
        }
        base.OnTriggerEnter2D(collision);
    }
    public void AfterJump()
    {
        rigidbody2d.velocity = new Vector2(0f, 0f);
        injump = false;
        unitbase = null;
        ChangeAttackUnit(0f);
    }

    public void OnGetStar()
    {
        getStar = true;
        injump = false;
        this.IgnoreSpecialAttack.Clear();
        this.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        this.Reverse = true;
        ChangeAttackUnit(0.01f);
    }

    public void OnStarBreack()
    {
        getStar = true;
        injump = false;
        this.IgnoreSpecialAttack.Clear();
        this.bufDetail.AddBuf(new ZombieBuf_DreamSeekerLoseStar());
        this.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
        ChangeAttackUnit(0.01f);
        MusicManage.Instance.PlayEffect((Random.Range(0f, 1f) > 0.5f) ? "newspaper_rarrgh" : "newspaper_rarrgh2", 1);
    }

    public override void DieAfterLoseHead()
    {
        if (injump)
        {
            this.Die();
            return;
        }
        base.DieAfterLoseHead();
    }
    public override void LoseArm()
    {
        transform.GetChild(0).Find("右大臂").GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).Find("右大臂").GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).Find("右大臂").GetComponent<SpriteRenderer>().sprite = Attribute.GetSprite("Zombie_polevaulter_outerarm_upper2");
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombiePolevaulterArm", transform.GetChild(0).Find("右大臂").GetChild(0).position).transform;
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.3f, 0.3f);
        base.LoseArm();
    }
    public override void LoseHead()
    {
        base.LoseHead();
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombiePolevaulterHead", transform.GetChild(0).Find("头").position).transform;
        float startspeed = Random.Range(0.5f, 1.5f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(startspeed, 5f);
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.transform.DOLocalRotate(new Vector3(0f, 0f, 180f * startspeed), 0.58f);
        transform.GetChild(0).Find("头").gameObject.SetActive(false);
        transform.GetChild(0).Find("杆前端").gameObject.SetActive(false);
        transform.GetChild(0).Find("左大臂").gameObject.SetActive(false);
    }
    public class ZombieBuf_DreamSeekerLoseStar : BattleUnitBuf
    {
        public override float SpeedChange()
        {
            return 1.2f;
        }
        public override void OnAdd()
        {
            base.OnAdd();
            _owner.ChangeSpeed();
        }
        public override void OnDestory()
        {
            _owner.ChangeSpeed();
            base.OnDestory();
        }
    }
}
