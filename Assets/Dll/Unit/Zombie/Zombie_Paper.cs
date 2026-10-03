using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Paper : ZombiesBase
{
    public GameObject face;
    public override void OnLoseArmor(Armor armor)
    {
        if(armor is Paper)
        {
            this.GetComponent<Animator>().CrossFade("LosePaper",0.1f);
            ChangeHand();
        }
    }
    public void ChangeHand()
    {
        this.transform.GetChild(0).Find("Zombie_paper_hands").gameObject.SetActive(false);
        this.transform.GetChild(0).Find("Zombie_paper_hands2").gameObject.SetActive(true);
        this.transform.GetChild(0).Find("Zombie_paper_hands3").gameObject.SetActive(true);
    }
    public void AfterLosePaper()
    {
        if(face != null)
        {
            face.SetActive(false);
        }
        MusicManage.Instance.PlayEffect((Random.Range(0f,1f)>0.5f)? "newspaper_rarrgh" : "newspaper_rarrgh2", 1);
        ChangeAttackUnit(0.2f);
    }
    public override void LoseArm()
    {
        base.LoseArm();
        this.transform.GetChild(0).Find("Zombie_paper_hands3").gameObject.SetActive(false);
        this.transform.GetChild(0).Find("Zombie_paper_leftarm_lower").gameObject.SetActive(false);
        this.transform.GetChild(0).Find("Zombie_paper_leftarm_upper").GetChild(0).gameObject.SetActive(false);
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombiePaperArm", transform.GetChild(0).Find("Zombie_paper_leftarm_lower").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.3f, 0.3f);
    }
    public override void LoseHead()
    {
        base.LoseHead();
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombiePaperHead", transform.GetChild(0).Find("Zombie_head").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        float startspeed = Random.Range(0.5f, 1.5f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(startspeed, 5f);
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.transform.DOLocalRotate(new Vector3(0f, 0f, 180f * startspeed), 0.58f);
        transform.GetChild(0).Find("Zombie_head").gameObject.SetActive(false);
        transform.GetChild(0).Find("Zombie_paper_hands2").gameObject.SetActive(false);
    }
}
