using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Generalattack : Armor
{
    public List<int> customzombie = new List<int> { 0,1,2,3,4,5,6 };
    public List<int> dreamzombie = new List<int> { 101, 102, 103, 106, 107, 108, 109 };
    public override void Init(ZombiesBase zombiesBase)
    {
        base.Init(zombiesBase);
        if (BattleManage.Instance.TimeRun <= 0) return;
        int num = 4;
        if (ZombieManage.Instance.nowflag % 10 == 0)
        {
            num = 6;
        }
        while(num > 0)
        {
            num--;
            int id = RandomUtil.SelectOne(customzombie);
            if (ZombieManage.Instance.nowflag % 10 == 0)
            {
                id = RandomUtil.SelectOne(dreamzombie);
            }
            GameObject obj = ZombieManage.Instance.InitZombie(id, zombiesBase.Line);
            ZombieManage.Instance.LoadZombieMess(obj, id, zombiesBase.Line);
            obj.transform.position = zombiesBase.transform.position + Vector3.right * Random.Range(0f, 2f);
            obj.GetComponent<Animator>().SetBool("Go", true);
        }
        RandomUtil.AddOrGetComponent<TimeDestory>( PoolManage.Instance.GetPoolGameObject("ParticleSystem", "ObjectCreate", this.transform.position)).Init(1f);
    }
}
