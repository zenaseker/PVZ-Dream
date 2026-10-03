using System;
using UnityEngine;

public enum Bullettype//子弹种类
{
    Cart,//小车
    Pea,//豌豆
    PuffShroom,//孢子
    Cherry,//樱桃
    PotatoMine,//土豆雷
    Zombie,//僵尸
    None,//无类型
    Icicle,//冰锥
    Light,//光束
    WallNutBall,//保龄球
    Droom,//毁灭
    Cattail,//猫尾草
    Chomper,//大嘴花
    Meteor,//陨石
    Buf,//Buf
}

public enum BulletFlyType//子弹移动方式
{
    Default,//平行直线
    Penetrate,//平行穿透
    Track,//追踪
    Air,//对空
    Pult,//投射物
    MapPpult,//全图投射物
}
public enum DamageElement//伤害属性
{
    Default,//普通
    Fire,//火焰
    Light,//光
    Snow,//冰
    Dark,//暗
    Soul,//灵
}

public enum UnitFaction//阵营
{
    Null,//无阵营
    Plant,//植物
    Zombie//僵尸
}


public class BulletBase : MonoBehaviour, IEnchantment
{
    public DamageObject DamageObject = null;//伤害体
    public int dmg = 20;//伤害
    public Vector2 flyspeed;//飞行速度
    public Bullettype type = Bullettype.None;//子弹类型
    public Rigidbody2D rigidbody2d;//钢体
    public BulletFlyType flyType = BulletFlyType.Default;//飞行类型
    public DamageElement damageType = DamageElement.Default;//伤害类型
    public BattleUnitModel attacker = null;//子弹来源
    public BattleUnitModel target = null;//子弹目标(追踪/投射)
    Vector3 startpos;//子弹终点(投射)
    Vector3 targetpos;//子弹终点(投射)
    float flytime = 0f;
    public UnitFaction faction = UnitFaction.Null;//来源阵营
    //起始行
    public int StartLine;//所在行
    protected bool isHit = false;//非穿透击中目标
    protected Transform shadow;//影子
    int GoLine = 0;//移动目标行

    public Action<BattleUnitModel,int> Action { get; set; }
    public virtual void FixedUpdate()
    {
        flytime += Time.fixedDeltaTime * flyspeed.x;
        switch (flyType)
        {
            case BulletFlyType.Default:
            case BulletFlyType.Penetrate:
                DefaultBulletUpdate();
                if (GoLine != StartLine && StartLine != -1)
                {
                    DefaultInChangeLineBulletUpdate();
                }
                break;
            case BulletFlyType.Track:
                TrackBulletUpdate();
                break;
            case BulletFlyType.Pult:
            case BulletFlyType.MapPpult:
                PultBulletUpdate();
                break;
        }
    }
    void DefaultBulletUpdate()
    {
        if (!IsInView())
        {
            PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject);
        }
    }
    void DefaultInChangeLineBulletUpdate()
    {
        if (Mathf.Abs(this.transform.position.y - MapManage.Instance.meshpos[GoLine, 0].y - 0.5f) < 0.05f)
        {
            Vector3 vector3 = this.transform.position;
            vector3.y = MapManage.Instance.meshpos[GoLine, 0].y + 0.5f;
            this.transform.position = vector3;
            StartLine = GoLine;
        }
        else
        {
            Vector3 vector3 = this.transform.position;
            vector3.y -= (this.transform.position.y - MapManage.Instance.meshpos[GoLine, 0].y - 0.5f) * 0.02f;
            this.transform.position = vector3;
        }
    }
    void TrackBulletUpdate()
    {
        if (!IsInView())
        {
            PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject);
        }
        if (target != null && !(target is ZombiesBase && (target as ZombiesBase).isdie))
        {
            Vector3 face = target.transform.position + Vector3.up - this.transform.position;
            float angle = -Vector2.Angle(face, Vector2.right);
            this.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angle);
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(face.x, face.y).normalized * flyspeed.x;
        }
    }
    void PultBulletUpdate()
    {
        if (flyType == BulletFlyType.Pult && this.transform.position.y < MapManage.Instance.meshpos[this.StartLine, 0].y - 0.5f)
        {
            this.OnHit(null);
        }
        if (attacker != null)
        {
            startpos = attacker.transform.position;
        }
        if (target != null)
        {
            targetpos = target.transform.position;
            for (float i = 0f; i < 1f; i += 0.05f)
            {
                Debug.DrawLine(BezierLine(i), BezierLine(i + 0.05f), Color.red);
            }
        }
        Vector3 face = BezierLine(flytime + Time.fixedDeltaTime) - BezierLine(flytime);
        float angle = Vector2.Angle(face, Vector2.right);
        this.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angle);
        this.transform.position = BezierLine(flytime);
    }
    Vector3 BezierLine(float t)
    {
        Vector3 uppos = (targetpos + startpos) / 2 + Vector3.up * 2;
        float t1 = Mathf.Pow(1 - t, 2);
        float t2 = 2 * t * (1 - t);
        float t3 = Mathf.Pow(t, 2);
        Vector3 pos = t1 * startpos + t2 * uppos + t3 * targetpos;
        return pos;
    }
    public bool IsInView()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (viewPos.x >= 0f && viewPos.x <= 1f && viewPos.y >= 0f && viewPos.y <= 1f) return true;
        return false;
    }
    public void ChangeLine(int line)
    {
        GoLine = line;
    }
    public void OnEnable()
    {
        startpos = this.transform.position;
        flytime = 0f;
    }
    public virtual void Init(Vector2 flyspeed, int startline, UnitFaction unitFaction, int dmg = -1)
    {
        if (dmg != -1)
        {
            this.dmg = dmg;
        }
        this.flyspeed = flyspeed;
        this.StartLine = GoLine = startline;
        this.faction = unitFaction;
        shadow = this.transform.Find("shadow");
        isHit = false;
        this.GetComponent<Collider2D>().enabled = true;
        if (this.flyType != BulletFlyType.Pult)
        {
            this.GetComponent<Rigidbody2D>().velocity = flyspeed;
        }
        Action = null;
        startpos = this.transform.position;
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.isHit) return;
        if (collision.gameObject.tag == "Map") return;
        if (collision.gameObject.tag == "Zombie")
        {
            if (collision.gameObject.GetComponent<ZombiesBase>().Reverse && faction == UnitFaction.Plant)
            {
                return;
            }
            if (!collision.gameObject.GetComponent<ZombiesBase>().Reverse && faction == UnitFaction.Zombie)
            {
                return;
            }
        }
        if (collision.gameObject.tag == "Plant" && faction != UnitFaction.Zombie)
        {
            return;
        }
        collision.gameObject?.GetComponent<BattleUnitModel>()?.TakeDamage(new DamageObject(this.dmg, type, attacker)
        {
            DamageElement = this.damageType,
        });
        OnHit(collision.gameObject.GetComponent<BattleUnitModel>());
    }
    public virtual void OnHit(BattleUnitModel unit)
    {
        HitEffect();
        if (this.flyType != BulletFlyType.Penetrate)
        {
            this.rigidbody2d.velocity = Vector2.zero;
            this.GetComponent<Collider2D>().enabled = false;
            this.isHit = true;
        }
        Action?.Invoke(unit, dmg);
        AfterHit();
    }
    public virtual void HitEffect()
    {
    }
    public virtual void AfterHit()
    {
    }
}
