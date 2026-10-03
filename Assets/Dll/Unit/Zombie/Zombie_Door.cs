using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Door : Zombie_Common
{
    public GameObject leftarm;
    public GameObject rightarm;
    public GameObject leftarm2;
    public GameObject rightarm2;
    public override void OnLoseArmor(Armor armor)
    {
        base.OnLoseArmor(armor);
        if (armor is Door)
        {
            this.Changearm(false);
        }
    }
    public void Changearm(bool flag)
    {
        rightarm2.SetActive(flag);
        leftarm2.SetActive(flag);
        leftarm.SetActive(!flag);
        rightarm.SetActive(!flag);
    }

    public void ChangeArm(int flag)
    {
        Changearm(flag == 0);
    }

    public override void StartDie()
    {
        base.StartDie();
        if (this.armor2 == null) return;
        this.Changearm(false);
    }
}
