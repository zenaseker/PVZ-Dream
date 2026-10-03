using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Pirate : ZombiesBase
{
    public Transform atkpos;
    int nearattackcount = 0;
    float movetime = 0f;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        movetime += Time.deltaTime;
        if (movetime >= 5f)
        {
            movetime = 0f;
            this.GetComponent<Animator>().SetTrigger("Far");
        }
    }
    public override void Attack()
    {
        base.Attack();
        nearattackcount++;
        if (nearattackcount >= 10)
        {
            nearattackcount = 0;
            movetime = 0f;
            this.GetComponent<Animator>().SetTrigger("Far");
        }
    }
    public void FarAttack()
    {
        if (losearm) return;
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "LeadBullet", atkpos.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(this.Reverse ? 7f : -7f, 0), this.Line, this.Reverse ? UnitFaction.Plant : UnitFaction.Zombie, this.Damage(100, DamageElement.Default));
    }
    public override void LoseArm()
    {
        base.LoseArm();
        this.transform.GetChild(0).Find("Zombie_paper_hands3").gameObject.SetActive(false);
        this.transform.GetChild(0).Find("Zombie_paper_leftarm_lower").gameObject.SetActive(false);
        this.transform.GetChild(0).Find("Zombie_paper_leftarm_upper").GetChild(0).gameObject.SetActive(false);
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombiePirateArm", transform.GetChild(0).Find("Zombie_paper_leftarm_lower").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.3f, 0.3f);
    }
    public override void LoseHead()
    {
        base.LoseHead();
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombiePirateHead", transform.GetChild(0).Find("Zombie_head").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        float startspeed = Random.Range(0.5f, 1.5f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(startspeed, 5f);
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.transform.DOLocalRotate(new Vector3(0f, 0f, 180f * startspeed), 0.58f);
        transform.GetChild(0).Find("Zombie_head").gameObject.SetActive(false);
    }
}
