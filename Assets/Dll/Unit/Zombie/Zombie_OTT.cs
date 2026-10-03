using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_OTT : Zombie_Common
{
    public bool Stop = false;
    float cooltime = 1f;

    protected override void OnUpdate()
    {
        base.OnUpdate();
        cooltime -= Time.deltaTime;
        if (cooltime <= 0)
        {
            CheckStop(false);
            cooltime = 1f;
        }
    }

    public void CheckStop(bool inshow)
    {
        if (inshow)
        {
            if (!Stop)
            {
                Stop = RandomUtil.Probability(0.5f, true);
                cooltime = 1f;
            }
        }
        else
        {
            Stop = RandomUtil.Probability(0.1f, true);
        }
        Changespeed(Stop);
    }
    void Changespeed(bool flag)
    {
        this.GetComponent<Animator>().speed = flag ? 0f : 2f;
    }
}
