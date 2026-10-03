using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;
using UnityEngine.UI;
using DG.Tweening;

public class Zombie_Jackson : ZombiesBase
{
    protected float moonWalkTime;

    protected GameObject[] dancer = new GameObject[4];

    protected bool isMoonWalkFinish;

    public override void Init(int order, ZombieInfo zombieCard, int line, float startspeed)
    {
        base.Init(order, zombieCard, line, startspeed);
        if (BattleManage.Instance.TimeRun > 0)
        {
            MusicManage.Instance.PlayEffect("dancer", 1f);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        moonWalkTime += Time.deltaTime;
        if (moonWalkTime > 3f && !isMoonWalkFinish)
        {
            this.GetComponent<Animator>().SetTrigger("summon");
            isMoonWalkFinish = true;
        }
        if (this.Line == 0)
        {
            if (dancer[1] == null || dancer[2] == null || dancer[3] == null)
            {
                this.GetComponent<Animator>().SetBool("loseDancer",true);
            }
        }
        else if (this.Line == 4)
        {
            if (dancer[0] == null || dancer[1] == null || dancer[2] == null)
            {
                this.GetComponent<Animator>().SetBool("loseDancer", true);
            }
        }
        else if (dancer[0] == null || dancer[1] == null || dancer[2] == null || dancer[3] == null)
        {
            this.GetComponent<Animator>().SetBool("loseDancer", true);
        }
    }

    public override void OnPlantContact(PlantBase plant)
    {
        if (!isMoonWalkFinish)
        {
            this.GetComponent<Animator>().SetTrigger("summon");
            isMoonWalkFinish = true;
        }
    }

    private void SetToatkState()
    {
        this.GetComponent<Animator>().SetBool("loseDancer", this.GetComponent<Animator>().GetBool("Attack"));
    }

    protected virtual void AnimSummon()
    {
        this.GetComponent<Animator>().SetBool("loseDancer", false);
        if (dancer[0] == null && this.Line != 0)
        {
            CreateParticle(0);
        }
        if (dancer[1] == null)
        {
            CreateParticle(1);
        }
        if (dancer[2] == null)
        {
            CreateParticle(2);
        }
        if (dancer[3] == null && this.Line != MapManage.Instance.meshxy.x - 1)
        {
            CreateParticle(3);
        }
    }

    public override void OnMindControl()
    {
        base.OnMindControl();
        dancer = new GameObject[4];
    }

    public override void LoseArm()
    {
        base.LoseArm();
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombieJackSonArm", transform.GetChild(0).Find("Zombie_Jackson_outerarm_lower").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        head.gameObject.AddComponent<TimeDestory>().Init(1f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.6f, 0.6f);
        this.transform.GetChild(0).Find("Zombie_Jackson_outerarm_hand").gameObject.SetActive(false);
        this.transform.GetChild(0).Find("Zombie_Jackson_outerarm_lower").gameObject.SetActive(false);
    }

    public override void LoseHead()
    {
        base.LoseHead();
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombieJackSonHead", this.transform.GetChild(0).Find("Zombie_jackson_head").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        float startspeed = Random.Range(0.5f, 1.5f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(startspeed, 5f);
        head.gameObject.AddComponent<TimeDestory>().Init(1f);
        head.transform.DOLocalRotate(new Vector3(0f, 0f, 180f * startspeed), 0.58f);
        this.transform.GetChild(0).Find("Zombie_Jackson_hair").gameObject.SetActive(false);
        this.transform.GetChild(0).Find("Zombie_jaw").gameObject.SetActive(false);
        this.transform.GetChild(0).Find("Zombie_jackson_head").gameObject.SetActive(false);
    }


    protected void CreateParticle(int summon,int id = 8)
    {
        GameObject obj = ZombieManage.Instance.InitZombie(id, this.Line);
        dancer[summon] = obj;
        switch (summon)
        {
            case 0:
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line - 1,this.speed);
                obj.transform.position = this.transform.position + Vector3.down * 2;
                break;
            case 1:
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line, this.speed);
                obj.transform.position = this.transform.position + Vector3.left * 2;
                break;
            case 2:
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line, this.speed);
                obj.transform.position = this.transform.position + Vector3.right * 2;
                break;
            case 3:
                ZombieManage.Instance.LoadZombieMess(obj, id, this.Line + 1, this.speed);
                obj.transform.position = this.transform.position + Vector3.up * 2;
                break;
        }
        obj.GetComponent<ZombiesBase>().ZombieUp();
        if (this.bufDetail.GetKeyWordBuf(KeyWordBuf.MindControl) != null)
        {
            obj.GetComponent<BattleUnitModel>().bufDetail.AddKeyWordBuf(KeyWordBuf.MindControl, 1);
        }
    }
}
