using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Flag : Zombie_Common
{
    GameObject flagarea;
    public GameObject youhand;
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed = 1f)
    {
        base.Init(order, zombieCard, line, startspeed);
        flagarea = PoolManage.Instance.GetPoolGameObject("Area", "ZombieFlagArea", this.transform.position);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnLoseArmor(Armor armor)
    {
        base.OnLoseArmor(armor);
        if (armor is Flag)
        {
            PoolManage.Instance.PushGameObject(flagarea.name, flagarea);
            this.transform.GetChild(0).Find("∆Ï÷ƒ”“±€").gameObject.SetActive(false);
            youhand.SetActive(true);
            flagarea = null;
        }
    }

    public override void Die()
    {
        if (flagarea != null)
        {
            PoolManage.Instance.PushGameObject(flagarea.name, flagarea);
        }
        base.Die();
    }
}

