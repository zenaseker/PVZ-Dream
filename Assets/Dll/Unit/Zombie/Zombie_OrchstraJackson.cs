using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_OrchstraJackson : Zombie_Jackson
{
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed)
    {
        base.Init(order, zombieCard, line, startspeed);
        this.GetComponent<Animator>().SetBool("loseDancer", true);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
    protected override void AnimSummon()
    {
        int[] num = new int[4] { 8, 8, 8, 8 };
        for(int i = 0;i < 3; i++)
        {
            if (BattleManage.Instance.SunNumber >= i * 1000)
            {
                num[i] = 7;
            }
        }
        if (this.Line != 0)
        {
            CreateParticle(0, num[0]);
        }
        CreateParticle(1, num[1]);
        CreateParticle(2, num[2]);
        if (this.Line != MapManage.Instance.meshxy.x - 1)
        {
            CreateParticle(3, num[3]);
        }
        for(int i = 0;i < GameObject.Find("ZombieManage").transform.childCount; i++)
        {
            if (GameObject.Find("ZombieManage").transform.GetChild(i).TryGetComponent<ZombiesBase>(out ZombiesBase value))
            {
                if (value.unitInfo.Prefab.name.Contains("Jackson"))
                {
                    value.ReCoverHp(BattleManage.Instance.SunNumber / 10);
                }
            }
        }
    }
}
