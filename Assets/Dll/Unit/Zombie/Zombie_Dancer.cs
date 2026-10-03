using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Dancer : ZombiesBase
{
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed = 1)
    {
        base.Init(order, zombieCard, line, startspeed);
        GameObject effect = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "PointUp", this.transform);
        effect.transform.localPosition = Vector3.zero;
        RandomUtil.AddOrGetComponent<TimeDestory>(effect).Init(1f);

    }
    public override void LoseArm()
    {
        base.LoseArm();
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombieDancerArm", transform.Find("Body").Find("Zombie_Dancer_outerarm_lower").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        head.gameObject.AddComponent<TimeDestory>().Init(1f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.6f, 0.6f);
        this.transform.Find("Body").Find("Zombie_Dancer_outerarm_hand").gameObject.SetActive(false);
        this.transform.Find("Body").Find("Zombie_Dancer_outerarm_lower").gameObject.SetActive(false);
    }

    public override void LoseHead()
    {
        base.LoseHead();
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombieDancerHead", this.transform.Find("Body").Find("Zombie_Dancer_head").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        float startspeed = Random.Range(0.5f, 1.5f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(startspeed, 5f);
        head.gameObject.AddComponent<TimeDestory>().Init(1f);
        head.transform.DOLocalRotate(new Vector3(0f, 0f, 180f * startspeed), 0.58f);
        this.transform.Find("Body").Find("Zombie_Dancer_hair").gameObject.SetActive(false);
        this.transform.Find("Body").Find("Zombie_jaw").gameObject.SetActive(false);
        this.transform.Find("Body").Find("Zombie_Dancer_head").gameObject.SetActive(false);
    }
}
