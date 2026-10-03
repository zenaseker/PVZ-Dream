using DG.Tweening;
using UnityEngine;

public class Zombie_Spear : ZombiesBase
{
    public bool Fired = false;
    public bool infire = false;
    public float deltatime = 5f;
    public void Fire()
    {
        if (unitbase != null && unitbase is PlantBase && ((Attribute.PlantInfo)unitbase.unitInfo).Planttype == PlantType.Defensive)
        {
            unitbase.TakeDamage(new DamageObject(300, Bullettype.Zombie, this));
        }
        else
        {
            GameObject fire = PoolManage.Instance.GetPoolGameObject("Bullet", "SpearFire", this.transform.GetChild(0).Find("∏À«∞∂À").transform.position);
            RandomUtil.AddOrGetComponent<SpearFire>(fire).Init(new Vector2(5.5f, 0), this.Line, UnitFaction.Zombie, 300);
        }
        rigidbody2d.velocity = new Vector2(0f, 0f);
        unitbase = null;
        transform.GetChild(0).Find("∏À«∞∂À").gameObject.SetActive(false);
        infire = false;
        ChangeAttackUnit(0.01f);
    } 

    protected override void OnUpdate()
    {
        base.OnUpdate();
        deltatime -= Time.deltaTime;
        if ((unitbase != null || deltatime <= 0f) && !this.Fired)
        {
            this.GetComponent<Animator>().SetBool("Attack", true);
            Fired = true;
            infire = true;
        }
    }

    public override void DieAfterLoseHead()
    {
        if (Fired)
        {
            this.Die();
            return;
        }
        base.DieAfterLoseHead();
    }
    public override void LoseArm()
    {
        transform.GetChild(0).Find("”“¥Û±€").GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).Find("”“¥Û±€").GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).Find("”“¥Û±€").GetComponent<SpriteRenderer>().sprite = Attribute.GetSprite("Zombie_polevaulter_outerarm_upper2");
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombiePolevaulterArm", transform.GetChild(0).Find("”“¥Û±€").GetChild(0).position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.3f, 0.3f);
        base.LoseArm();
    }
    public override void LoseHead()
    {
        base.LoseHead();
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombiePolevaulterHead", transform.GetChild(0).Find("Õ∑").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        float startspeed = Random.Range(0.5f, 1.5f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(startspeed, 5f);
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.transform.DOLocalRotate(new Vector3(0f, 0f, 180f * startspeed), 0.58f);
        transform.GetChild(0).Find("Õ∑").gameObject.SetActive(false);
        transform.GetChild(0).Find("∏À«∞∂À").gameObject.SetActive(false);
        transform.GetChild(0).Find("◊Û¥Û±€").gameObject.SetActive(false);
    }
}
