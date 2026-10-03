using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Berserker : Zombie_Paper
{
    public override void OnLoseArmor(Armor armor)
    {
        if (armor is berserkerhat)
        {
            this.GetComponent<Animator>().CrossFade("LosePaper", 0.1f);
            this.unitInfo.Damage += 50;
        }
    }
    public override void LoseArm()
    {
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombieBerserkerArm", transform.GetChild(0).Find("Zombie_berserker_leftarm_lower").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.3f, 0.3f);
        this.transform.GetChild(0).Find("Zombie_berserker_hands3").gameObject.SetActive(false);
        this.transform.GetChild(0).Find("Zombie_berserker_leftarm_lower").gameObject.SetActive(false);
        this.transform.GetChild(0).Find("Zombie_berserker_leftarm_upper").gameObject.SetActive(false);
        this.losearm = true;
    }
    public override void LoseHead()
    {
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombieCommonHead", transform.GetChild(0).Find("Zombie_head").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        float startspeed = Random.Range(0.5f, 1.5f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(startspeed, 5f);
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.transform.DOLocalRotate(new Vector3(0f, 0f, 180f * startspeed), 0.58f);
        transform.GetChild(0).Find("Zombie_head").gameObject.SetActive(false);
        this.losehead = true;
        if (isdie) return;
        Invoke("DieAfterLoseHead", 2f);
    }
}
